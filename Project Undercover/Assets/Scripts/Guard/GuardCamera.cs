using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCamera : Photon.PunBehaviour {

    public float xRotation = 0.0f;
    public float yRotation = 0.0f;
    public Light spotLight;

    void Start () {
        xRotation = transform.eulerAngles.y;
        yRotation = transform.eulerAngles.x;
    }
	
	void Update () {
        Quaternion newRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
        newRotation *= Quaternion.AngleAxis(-yRotation, -Vector3.right);
        transform.localRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.time * 0.01f);
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

    [PunRPC]
    void SetEnabledRPC(bool enabled)
    {
        spotLight.enabled = enabled;
    }
}
