using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCameraController : Photon.PunBehaviour {

	private float camSens = 120.0f;
    private GuardCamera cam;

    static List<GuardCameraController> cameras;
    static int currentCamera;

	void Start () {
        cam = GetComponent<GuardCamera>();
        if (cameras == null)
        {
            cameras = new List<GuardCameraController>();
            foreach (var camera in GameObject.FindGameObjectsWithTag(tag))
            {
                cameras.Add(camera.GetComponent<GuardCameraController>());
            }
            for (int i=0; i < cameras.Count; i++)
            {
                if (cameras[i] == this)
                    currentCamera = i;
                else
                    SetCameraEnabled(cameras[i], false);
            }
        }
    }

    GuardCameraController GetCurrentCamera()
    {
        return cameras[currentCamera];
    }

    GuardCameraController GetNextCamera(out int nextCameraPos)
    {
        nextCameraPos = (currentCamera + 1) % cameras.Count;
        return cameras[nextCameraPos];
    }
	
    void SwitchCamera()
    { 
        var nextCam = GetNextCamera(out currentCamera);
        SetCameraEnabled(nextCam, true);
        SetCameraEnabled(this, false);
    }

    public void SetCameraEnabled(GuardCameraController gCamera, bool enabled)
    {
        gCamera.GetComponent<Camera>().enabled = enabled;
        gCamera.GetComponent<AudioListener>().enabled = enabled;
        gCamera.enabled = enabled;
        gCamera.photonView.RPC("SetEnabledRPC", PhotonTargets.Others, enabled);
    }

	void Update () {
        if (Input.GetKeyDown("space"))
            SwitchCamera();

        RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.parent.gameObject.tag == "NPC")
                {
                    var manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    manager.photonView.RPC("ShowSpiesWinScreen", PhotonTargets.All);
                }
                else if (hit.transform.parent.gameObject.tag == "Spy")
                {
                    var manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    manager.photonView.RPC("ShowGuardsWinScreen", PhotonTargets.All);
                }
            }
        }

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            return;

        cam.xRotation += Time.deltaTime * Input.GetAxis("Horizontal") * camSens;
        cam.yRotation += Time.deltaTime * -Input.GetAxis("Vertical") * camSens;
        cam.xRotation = cam.xRotation % 360;
        cam.yRotation = Mathf.Clamp(cam.yRotation, -45, 80);
        cam.UpdateRotation();
	}
}