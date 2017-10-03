using UnityEngine;
using System;

public class CharacterAnimator : Photon.PunBehaviour
{
    private Animator animator;
    private int[] paramHashes;
    private StateController controller;

    public enum Params
    {
        MoveSpeed = 0, PassingMessage, Interrupted
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<StateController>();

        // Map param-enums to param-hashes with paramHashes Array
        string[] enumNames = Enum.GetNames(typeof(Params));
        paramHashes = new int[enumNames.Length];

        for (int i = 0; i < paramHashes.Length; i++)
        {
            paramHashes[i] = Animator.StringToHash(enumNames[i]);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(controller.navMeshAgent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat(paramHashes[(int)Params.MoveSpeed], (float)stream.ReceiveNext());
        }
    }

    protected virtual void Update()
    {
        animator.SetFloat(paramHashes[(int)Params.MoveSpeed], controller.navMeshAgent.velocity.magnitude);
    }

    public bool GetTrigger(Params param)
    {
        return animator.GetBool(paramHashes[(int)param]);
    }

    public void SetTrigger(Params param)
    {
        photonView.RPC("SetTriggerRPC", PhotonTargets.All, paramHashes[(int)param], true);
    }

    public void ResetTrigger(Params param)
    {
        photonView.RPC("SetTriggerRPC", PhotonTargets.All, paramHashes[(int)param], false);
    }

    [PunRPC]
    void SetBoolRPC(int hash, bool value)
    {
        animator.SetBool(hash, value);
    }

    [PunRPC]
    void SetTriggerRPC(int hash, bool value)
    {
        if (value)
            animator.SetTrigger(hash);
        else
            animator.ResetTrigger(hash);
    }

    public static string GetParamName(Params param)
    {
        return Enum.GetName(typeof(Params), param);
    }

}
