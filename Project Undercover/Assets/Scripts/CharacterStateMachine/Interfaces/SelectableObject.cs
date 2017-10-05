using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : Photon.PunBehaviour
{
    // PhotonView Id of interacting character
    [SerializeField]
    private int _queuedInteractorId = -1;
    [SerializeField]
    private bool _isInteracting = false;

    // Interactions NPC's or spies can perform on this object
    public Interaction[] interactions;

    // Interactions only spies can perform on this object
    public Interaction[] spyInteractions;

    public virtual void Selected()
    {
    }

    public virtual void Deselected()
    {
    }

    public virtual void Update()
    {
        if (Interactor != null)
        {
            AcceptInteraction();
        }
    }

    public bool HasInteractions()
    {
        return (interactions.Length + spyInteractions.Length) > 0;
    }

    public virtual string GetInteractionTitle()
    {
        return "";
    }

    // Manages other StateControllers signalling this controller for an interaction
    public StateController Interactor
    {
        get
        {
            if (_queuedInteractorId < 0)
                return null;
            PhotonView view = PhotonView.Find(_queuedInteractorId);
            if (view)
                return view.GetComponent<StateController>();
            else
                return null;
        }
        set
        {
            if (value == null)
            {
                photonView.RPC("SetInteractorRPC", PhotonTargets.All, -1);
                IsInteracting = false;
                return;
            }
            photonView.RPC("SetInteractorRPC", PhotonTargets.All, value.GetComponent<PhotonView>().viewID);
        }
    }

    public void AcceptInteraction()
    {
        IsInteracting = true;
    }

    public void RejectInteraction()
    {
        Interactor.IsInteracting = false;
    }

    public bool IsInteracting {
        get
        {
            return _isInteracting;
        }
        set
        {
            photonView.RPC("SetIsInteractingRPC", PhotonTargets.All, value);
        }
    }

    [PunRPC]
    protected void SetIsInteractingRPC(bool value)
    {
        _isInteracting = value;
    }

    [PunRPC]
    protected void SetInteractorRPC(int viewId)
    {
        _queuedInteractorId = viewId;
    }
}
