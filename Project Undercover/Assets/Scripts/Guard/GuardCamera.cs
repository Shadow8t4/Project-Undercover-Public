using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCamera : Photon.PunBehaviour {

    public float xRotation = 0.0f;
    public float yRotation = 0.0f;
    public bool laserSightEnabled = false;
    private GameObject line;

    void Start () {
        xRotation = transform.eulerAngles.y;
        yRotation = transform.eulerAngles.x;
    }
	
	void Update () {
        Quaternion newRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
        newRotation *= Quaternion.AngleAxis(-yRotation, -Vector3.right);
        transform.localRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.time * 0.01f);

        if (laserSightEnabled)
        {
            if (line == null)
                line = LineDrawer.MakeLine();
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
                LineDrawer.DrawLine(line, transform.position, hit.point);
            else
                LineDrawer.DrawLine(line, transform.position, transform.position + transform.forward * 20.0f);
        }
        else
        {
            if (line != null)
                Destroy(line);
        }
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
        laserSightEnabled = enabled;
    }
}
