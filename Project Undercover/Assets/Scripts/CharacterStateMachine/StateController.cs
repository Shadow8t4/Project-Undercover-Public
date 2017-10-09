using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : SelectableObject
{

    public State currentState;
    public State remainState;
    public static readonly float INTERACT_RANGE = 1.5f;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimator characterAnimator;

    // Object Selector properties
    private SelectableObject _selectedObject;
    private Interaction _selectedInteraction;
    private Coroutine _roamCoroutine;
    private static float _startInteractionProgressLimit = 0.3f;
    private static float _endInteractionProgressLimit = 0.8f;


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
        if (nextState != remainState)
        {
            currentState.DoEndActions(this);
            currentState = nextState;
            currentState.DoStartActions(this);
        }
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

    // Initiates interaction with other StateController
    public void InitiateInteractionWithSelectedObject()
    {
        if (SelectedObject == this)
            Debug.LogError("Attempted to set interactor as self");

        if (SelectedObject.IsInteracting)
            Debug.Log(SelectedObject.name + " is busy and cannot interact with " + name);

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
        var randTarget = new Vector3(roomSize - (roomSize * 2 * UnityEngine.Random.value), 0.0f, roomSize - (roomSize * 2 * UnityEngine.Random.value));
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
                hash = value.GetHashCode();
            photonView.RPC("SetSelectedInteractionRPC", PhotonTargets.All, hash);
        }
    }

    [PunRPC]
    private void SetSelectedInteractionRPC(int hash)
    {
        Interaction[] foundInteractions = (Interaction[])Resources.FindObjectsOfTypeAll(typeof(Interaction));
        foreach (var interaction in foundInteractions)
        {
            if (interaction.GetHashCode() == hash)
            {
                _selectedInteraction = interaction;
                return;
            }
        }
        _selectedInteraction = null;
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
            yield return new WaitForSeconds(UnityEngine.Random.value * 10.0f);
            Destination = GetRandomLocation();
        }
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
            transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, Time.deltaTime * 10.0f);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, facingRotation, Time.deltaTime * 10.0f);
        }
    }
}
