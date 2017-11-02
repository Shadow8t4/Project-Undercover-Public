using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : SelectableObject
{

    public State currentState;
    public static readonly float INTERACT_RANGE = 1.8f;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimator characterAnimator;

    // Object Selector properties
    private SelectableObject _selectedObject;
    private Interaction _selectedInteraction;
    private Coroutine _roamCoroutine;
    private static float _startInteractionProgressLimit = 0.2f;
    private static float _endInteractionProgressLimit = 1.0f;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (!photonView.isMine)
        {
            navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            navMeshAgent.updateRotation = false;
        }
        animator = GetComponent<Animator>();
        characterAnimator = GetComponent<CharacterAnimator>();
        if (PhotonNetwork.isMasterClient)
        {
            Color color = NpcColors.GetAvailableColor();
            Vector3 colorAsVector = new Vector3(color.r, color.g, color.b);
            photonView.RPC("SetCharacterColorRPC", PhotonTargets.All, colorAsVector);
        }
    }

    protected override void Start()
    {
        base.Start();
        if (photonView.isMine)
            currentState.DoStartActions(this);
    }

    protected override void Update()
    {
        base.Update();
        if (photonView.isMine)
        {
            currentState.UpdateState(this);
        }
    }

    public void TransitionToState(State nextState)
    {
        currentState.DoEndActions(this);
        currentState = nextState;
        currentState.DoStartActions(this);
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Destination);
            stream.SendNext(navMeshAgent.stoppingDistance);
        }
        else
        {
            Destination = (Vector3)stream.ReceiveNext();
            navMeshAgent.stoppingDistance = (float)stream.ReceiveNext();
        }
    }

    public Vector3 Destination
    {
        get
        {
            return navMeshAgent.destination;
        }
        set
        {
            navMeshAgent.destination = value;
        }
    }

    public void InitiateInteractionWithSelectedObject()
    {
        if (SelectedObject == this)
            Debug.LogError("Attempted to set interactor as self");

        if (SelectedObject.IsInteracting)
            return;

        // Debug.Log("Sending interaction request...");
        IsInteracting = true;
        SelectedObject.Interactor = this;
    }

    public SelectableObject SelectedObject
    {
        get
        {
            return _selectedObject;
        }
        set
        {
            if (_selectedObject != null)
            {
                _selectedObject.Deselected();
            }
            _selectedObject = value;
            if (_selectedObject != null)
            {
                _selectedObject.Selected();
                navMeshAgent.stoppingDistance = INTERACT_RANGE * 0.8f;
            }
            else
            {
                navMeshAgent.stoppingDistance = 0.0f;
                Destination = transform.position;
            }
        }
    }

    public override string GetInteractionTitle()
    {
        return name;
    }

    public static Vector3 GetRandomLocation()
    {
        float roomSize = 10.0f;
        var randTarget = new Vector3(roomSize - (roomSize * 2 * UnityEngine.Random.value), 0.1f, roomSize - (roomSize * 2 * UnityEngine.Random.value));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randTarget, out hit, 1.0f, NavMesh.AllAreas))
            return hit.position;
        else
            return GetRandomLocation();
    }

    public bool IsInteractionAccepted()
    {
        return IsInteracting && SelectedObject.IsInteracting && SelectedObject.Interactor == this;
    }
    public bool IsInteractionRejected()
    {
        return SelectedObject.Interactor != this;
    }

    public void FinishInteraction()
    {
        if (IsInteracting)
            IsInteracting = false;
        if (SelectedObject)
        {
            if (SelectedObject.IsInteracting)
            {
                SelectedObject.IsInteracting = false;
            }
            SelectedObject.Interactor = null;
            SelectedObject = null;
        }
    }

    public Interaction SelectedInteraction
    {
        get
        {
            return _selectedInteraction;
        }
        set
        {
            int hash = 0;
            if (value != null)
                hash = value.GetHash();
            photonView.RPC("SetSelectedInteractionRPC", PhotonTargets.All, hash);
        }
    }

    public void StartRoaming()
    {
        _roamCoroutine = StartCoroutine(Roam());
    }

    public void StopRoaming()
    {
        StopCoroutine(_roamCoroutine);
    }

    private IEnumerator Roam()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.value * 10.0f + 0.5f);
            if (IsInteracting)
                Debug.LogError("Should have stopped this coroutine by now");
            if (UnityEngine.Random.value < 0.2f)
            {
                SelectableObject randomObject = GetRandomAvailableSelectableObject();
                if (randomObject != null)
                {
                    SelectedObject = randomObject;
                    IsInteracting = true;
                    Interaction randomInteraction = SelectedObject.GetRandomNpcInteraction();
                    SelectedInteraction = randomInteraction;
                    yield return null;
                }
            }
            Destination = GetRandomLocation();
        }
    }

    private SelectableObject GetRandomAvailableSelectableObject(int tries=0)
    {
        int randomIndex = UnityEngine.Random.Range(0, objects.Count);
        SelectableObject randomObject = objects[randomIndex];
        if (randomObject == this && randomObject.HasNpcInteractions() && !randomObject.Interactor && !randomObject.IsInteracting)
        {
            if (tries < 3)
                return GetRandomAvailableSelectableObject(tries++);
            else
                return null;
        }

        return objects[randomIndex];
    }

    public void FaceSelectedObject()
    {
        float progress = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Vector3 otherPos = SelectedObject.transform.position;
        float initialRotation = SelectedInteraction.initialRotation;
        SetFacingRotation(otherPos, progress, initialRotation);
    }

    public void FaceInteractor()
    {
        if (Interactor == null)
            return;

        float progress = Interactor.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Vector3 otherPos = Interactor.transform.position;
        if (Interactor.SelectedInteraction == null)
            return;
        float initialRotation = Interactor.SelectedInteraction.objectInitialRotation;
        SetFacingRotation(otherPos, progress, initialRotation);
    }

    private void SetFacingRotation(Vector3 otherPos, float progress, float initialRotation)
    {
        if (progress > _startInteractionProgressLimit && progress < _endInteractionProgressLimit)
            return;

        Vector3 pos = transform.position;
        Vector3 facingDirection = (otherPos - pos).normalized;
        Quaternion facingRotation = Quaternion.FromToRotation(Vector3.forward, facingDirection);

        if (progress < _startInteractionProgressLimit)
        {
            Quaternion adjustedRotation = facingRotation * Quaternion.Euler(0, initialRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, progress * (1.0f / _startInteractionProgressLimit));
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, facingRotation, (progress - _endInteractionProgressLimit) * (1.0f / (1 - _endInteractionProgressLimit)));
        }
    }

    public void MoveForSelectedObjectInteraction()
    {
        Vector3 awayDirection = (transform.position - SelectedObject.transform.position).normalized;
        Vector3 newPos = SelectedObject.transform.position + awayDirection * SelectedInteraction.interactionDistance;
        navMeshAgent.stoppingDistance = 0.0f;
        Destination = newPos;
    }

    public bool InRangeOfSelectedObject()
    {
        return (SelectedObject.transform.position - transform.position).magnitude < INTERACT_RANGE;
    }

    #region RPC definitions
    [PunRPC]
    private void SetCharacterColorRPC(Vector3 color)
    {
        Material coloredMat = transform.Find("Man_Standing").GetComponent<Renderer>().material;
        coloredMat.color = new Color(color.x, color.y, color.z);
    }

    [PunRPC]
    private void SetSelectedInteractionRPC(int hash)
    {
        Interaction[] foundInteractions = (Interaction[])Resources.FindObjectsOfTypeAll(typeof(Interaction));
        foreach (var interaction in foundInteractions)
        {
            if (interaction.CompareHash(hash))
            {
                _selectedInteraction = interaction;
                return;
            }
        }
        _selectedInteraction = null;
    }
    #endregion
}
