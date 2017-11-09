using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardController : Photon.PunBehaviour {

    public Text cameraStatusText;

	const float CAMERA_SENSITIVITY = 120.0f;

    private int mCurrentCamera; // -1 means preview mode
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

        // Default is preview mode.
        mCurrentCamera = -1;
        EnablePreviewMode();
    }

    /**
     * Disable preview mode on all cameras.
     */
    void DisablePreviewMode()
    {
        if (mCurrentCamera >= 0)
        {
            return;
        }
        foreach (var guardCamera in mCameras)
        {
            guardCamera.DisablePreviewMode();
        }
    }

    /**
     * Enable preview mode on call cameras and position them appropriately.
     */
    void EnablePreviewMode()
    {
        // Deactivate current camera, if necessary.
        if (mCurrentCamera != -1)
        {
            mCameras[mCurrentCamera].Deactivate();
        }
        mCurrentCamera = -1;

        // Cameras are always arranged in ZxZ rows and columns. Determine Z.
        int z = (int) Mathf.Ceil(Mathf.Sqrt(mCameras.Count));
        float size = 1.0f / z;

        // Set all cameras to preview mode appropriately.
        for (int i = 0; i < mCameras.Count; ++i)
        {
            mCameras[i].EnablePreviewMode(size, i % z, i / z);
        }
    }

    /**
     * Set the text in the bottom-right corner, denoting if the camera is locked.
     */
    void SetCameraText()
    {
        // TODO - GuardCamera should be responsible for text.
        // Each GuardCamera should display its name in colored text.
        // Red denotes that it is owned by another player, Green denotes free.
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
     * Cycle the player to a specific camera, or just the next one.
     */
    void SwitchCamera(int nextCamera = -1)
    {
        // Get relevant camera indexes
        int lastCamera = mCurrentCamera;
        mCurrentCamera = nextCamera == -1 ? mCurrentCamera + 1 : nextCamera;
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

    GuardCamera GetCurrentGuardCamera()
    {
        return mCameras[mCurrentCamera];
    }

    void Update()
    {
        if (mCurrentCamera < 0)
        {
            UpdatePreviewMode();
        }
        else
        {
            UpdateSingle();
        }
	}

    void UpdatePreviewMode()
    {
        // If the player clicks a camera, switch to that camera.
        if (Input.GetButtonDown("Fire1"))
        {
            // Get the click location.
            float x = Input.mousePosition.x / Screen.width;
            float y = Input.mousePosition.y / Screen.height;

            // Cameras are always arranged in ZxZ rows and columns. Determine Z.
            int z = (int)Mathf.Ceil(Mathf.Sqrt(mCameras.Count));
            float size = 1.0f / z;

            // Determine the camera that the click corresponds to, if any.
            int tileX = (int) (x / size);
            int tileY = (int) (y / size);
            int camera = tileY * z + tileX;

            // If the camera is valid, switch to it.
            if (camera < mCameras.Count)
            {
                DisablePreviewMode();
                SwitchCamera(camera);
            }
        }
    }

    void UpdateSingle()
    {
        // Escape switches to Preview Mode.
        if (Input.GetKeyDown("escape"))
        {
            EnablePreviewMode();
            return;
        }

        // Space switches to next camera.
        if (Input.GetKeyDown("space"))
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

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = GetCurrentGuardCamera().mCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "NPC")
                {
                    ScorePanelController.GuardCaughtNPC();
                }
                else if (hit.transform.gameObject.tag == "Spy")
                {
                    ScorePanelController.CaughtSpy();
                }
            }
        }

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            return;
        }

        float xRotation = Time.deltaTime * Input.GetAxis("Horizontal") * CAMERA_SENSITIVITY;
        float yRotation = Time.deltaTime * -Input.GetAxis("Vertical") * CAMERA_SENSITIVITY;
        GetCurrentGuardCamera().Rotate(xRotation, yRotation);
	}
}