using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCamera : Photon.PunBehaviour {

    public float xRotation = 0.0f;
    public float yRotation = 0.0f;

    void Start () {
        xRotation = transform.eulerAngles.y;
        yRotation = transform.eulerAngles.x;
    }
	
	void Update () {
        transform.localRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(-yRotation, -Vector3.right);
    }

    public void UpdateRotation()
    {
        photonView.RPC("UpdateRotation", PhotonTargets.All, xRotation, yRotation);
    }

    [PunRPC]
    void UpdateRotation(float xRotation, float yRotation)
    {
        this.xRotation = xRotation;
        this.yRotation = yRotation;
    }
}
