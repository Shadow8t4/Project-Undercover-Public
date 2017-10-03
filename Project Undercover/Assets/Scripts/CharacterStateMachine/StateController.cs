using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : SelectableObject
{

    public State currentState;
    public State remainState;
    public State idleState;
    public float INTERACT_RANGE = 2.0f;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimator characterAnimator;

    [HideInInspector] private SelectableObject selectedObject;

    // PhotonView Id of interacting character
    [HideInInspector] private int queuedInteractorId;

    // Text describing the type of interaction
    [HideInInspector] public string interactionText;
    public GameObject wayPointPrefab;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    public void Start()
    {
        if (photonView.isMine)
            currentState.DoStartActions(this);
    }

    public void Update()
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
        }
        else
        {
            Destination = (Vector3)stream.ReceiveNext();
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

    // Manages other StateControllers signalling this controller for an interaction
    public StateController Interactor
    {
        get
        {
            if (queuedInteractorId < 0)
                return null;
            PhotonView view = PhotonView.Find(queuedInteractorId);
            if (view)
                return view.GetComponent<StateController>();
            else
                return null;
        }
        set
        {
            queuedInteractorId = value.GetComponent<PhotonView>().viewID;
        }
    }

    // Initiates interaction with other StateController
    public void InteractWithController(StateController controller)
    {
        if (controller != this)
            controller.Interactor = this;
        else
            Debug.LogError("Attempted to set interactor as self");
    }

    public SelectableObject SelectedObject
    {
        get
        {
            return selectedObject;
        }
        set
        {
            if (selectedObject != null)
                selectedObject.Deselected();
            selectedObject = value;
            if (selectedObject != null)
            {
                selectedObject.Selected();
                navMeshAgent.stoppingDistance = 1.0f;
            }
            else
            {
                navMeshAgent.stoppingDistance = 0.3f;
                Destination = transform.position;
            }
        }
    }

    public override string GetInteractionTitle()
    {
        return "Player";
    }
}
   