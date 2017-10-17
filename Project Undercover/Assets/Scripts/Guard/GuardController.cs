using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardController : Photon.PunBehaviour {

    public Text cameraStatusText;

	const float CAMERA_SENSITIVITY = 120.0f;

    private int mCurrentCamera;
    private List<GuardCamera> mCameras;

    private bool mInControl;

	void Start()
    {
        // Fetch all cameras
        mCameras = new List<GuardCamera>();
        foreach (var camera in GameObject.FindGameObjectsWithTag("GuardCamera"))
        {
            var component = camera.GetComponent<GuardCamera>();
            if (component != null)
            {
                mCameras.Add(component);
            }
        }

        // Default is no camera. First to get it in Update wins.
        // Photon doesn't seem to sync camera players until update,
        // so if we Activate() here, it won't lock.
        mCurrentCamera = -1;
    }

    /**
     * Set the text in the bottom-right corner, denoting if the camera is locked.
     */
    void SetCameraText()
    {
        if (mInControl)
        {
            cameraStatusText.text = "In-Control";
        }
        else
        {
            cameraStatusText.text = "Locked";
        }
    }

    /**
     * Cycle the player to the next camera, or grab the first camera
     * if we don't have one yet.
     */
    void SwitchCamera()
    {
        // Get relevant camera indexes
        int lastCamera = mCurrentCamera;
        mCurrentCamera += 1;
        if (mCurrentCamera >= mCameras.Count)
        {
            mCurrentCamera = 0;
        }

        // Deactivate the old camera, activate new one
        mCameras[mCurrentCamera].Activate();
        if (lastCamera != -1)
        {
            mCameras[lastCamera].Deactivate();
        }

        // Determine if we are in control.
        mInControl = mCameras[mCurrentCamera].InControl();
        SetCameraText();
    }

	void Update()
    {
        if (Input.GetKeyDown("space") || mCurrentCamera == -1)
        {
            SwitchCamera();
        }

        // Update camera status text if necessary.
        bool newInControl = mCameras[mCurrentCamera].InControl();
        if (newInControl != mInControl)
        {
            mInControl = newInControl;
            SetCameraText();
        }

        /*RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "NPC")
                {
                    var manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    manager.photonView.RPC("ShowSpiesWinScreen", PhotonTargets.All);
                }
                else if (hit.transform.gameObject.tag == "Spy")
                {
                    var manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    manager.photonView.RPC("ShowGuardsWinScreen", PhotonTargets.All);
                }
            }
        }*/

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            return;
        }

        float xRotation = Time.deltaTime * Input.GetAxis("Horizontal") * CAMERA_SENSITIVITY;
        float yRotation = Time.deltaTime * -Input.GetAxis("Vertical") * CAMERA_SENSITIVITY;
        mCameras[mCurrentCamera].Rotate(xRotation, yRotation);
	}
}