using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableObject : Photon.PunBehaviour, IEquatable<SelectableObject>
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

    public static List<SelectableObject> objects;

    private Color AvailableColor, InteractingColor;
    private bool isMousedOver = false;
    private float LerpFactor = 10;
    private List<Material> _materials = new List<Material>();
    private Color _currentColor;
    private bool isSpy;

    public Renderer[] Renderers
    {
        get;
        private set;
    }

    public Color CurrentColor
    {
        get { return _currentColor; }
    }

    public bool IsInteracting
    {
        get
        {
            return _isInteracting;
        }
        set
        {
            photonView.RPC("SetIsInteractingRPC", PhotonTargets.All, value);
        }
    }

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

    protected virtual void Start()
    {
        if (objects == null)
            objects = new List<SelectableObject>();
        objects.Add(this);
        isSpy = CompareTag("Spy");
        Renderers = GetComponentsInChildren<Renderer>();
        AvailableColor = Color.green;
        InteractingColor = Color.yellow;
        foreach (var renderer in Renderers)
        {
            _materials.AddRange(renderer.materials);
        }
    }

    protected virtual void OnDestroy()
    {
        objects.Remove(this);
    }

    public Color TargetColor
    {
        get
        {
            if (isMousedOver)
            {
                if (Interactor)
                    return InteractingColor;
                else
                    return AvailableColor;
            }
            else
            {
                Color color = Color.black;
                color.a = 0.0f;
                return color;
            }
        }
    }

    protected void LerpColors()
    {
        _currentColor = Color.Lerp(_currentColor, TargetColor, Time.deltaTime * LerpFactor);
        for (int i = 0; i < _materials.Count; i++)
        {
            _materials[i].SetColor("_GlowColor", _currentColor);
        }
    }

    protected virtual void Update()
    {
        LerpColors();
    }

    private void OnMouseEnter()
    {
        if (!isSpy)
            isMousedOver = true;
        else if (!photonView.isMine)
            isMousedOver = true;

    }

    private void OnMouseExit()
    {
        isMousedOver = false;
    }

    public virtual void Selected()
    {
    }

    public virtual void Deselected()
    {
    }

    public bool HasInteractions()
    {
        return (interactions.Length + spyInteractions.Length) > 0;
    }

    public bool HasNpcInteractions()
    {
        return (interactions.Length) > 0;
    }

    public bool HasSpyInteractions()
    {
        return (spyInteractions.Length) > 0;
    }

    public Interaction GetRandomNpcInteraction()
    {
        if (interactions.Length <= 0)
            Debug.LogError("Attempted to get a random npc interaction when " + name + " doesn't have any");

        int randomIndex = UnityEngine.Random.Range(0, interactions.Length);
        return interactions[randomIndex];
    }

    public virtual string GetInteractionTitle()
    {
        return "";
    }

    public void AcceptInteraction()
    {
        IsInteracting = true;
    }

    public void RejectInteraction()
    {
        Interactor.IsInteracting = false;
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

    public virtual bool Equals(SelectableObject other)
    {
        return photonView.viewID == other.photonView.viewID;    
    }
}
