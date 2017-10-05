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

    public void Start()
    {
        if (photonView.isMine)
            currentState.DoStartActions(this);
    }

    public override void Update()
    {
        if (photonView.isMine)
        {
            currentState.UpdateState(this);
            if (SelectedObject)
                Destination = SelectedObject.transform.position;
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

        Debug.Log("Sending interaction request...");
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
                _selectedObject.Deselected();
            _selectedObject = value;
            if (_selectedObject != null)
            {
                _selectedObject.Selected();
                navMeshAgent.stoppingDistance = INTERACT_RANGE;
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

    public void FinishInteraction()
    {
        IsInteracting = false;
        SelectedObject.Interactor = null;
        SelectedObject = null;
    }

    public Interaction SelectedInteraction
    {
        get
        {
            return _selectedInteraction;
        }
        set
        {
            _selectedInteraction = value;
        }
    }

    public void FaceInteractor()
    {
        if (Interactor == null)
            Debug.LogError("Cannot face a null Interactor");

        transform.LookAt(Interactor.transform);
    }

    public void FaceSelectedObject()
    {
        if (SelectedObject == null)
            Debug.LogError("Cannot face a null SelectedObject");

        transform.LookAt(SelectedObject.transform);
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
}
