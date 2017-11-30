using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardCamera : Photon.PunBehaviour {

    const string AVAILABLE_TEXT = "(Available)";
    const string IN_CONTROL_TEXT = "(In-Control)";
    const string LOCKED_TEXT = "(Locked)";

    Color AVAILABLE_COLOR = Color.black;
    Color IN_CONTROL_COLOR = Color.black;
    Color LOCKED_COLOR = Color.red;

    public Text cameraStatusText;
    
    public Camera Camera { get { return mCamera; } }

    private Camera mCamera;
    private Light mSpotlight;
    private AudioListener mListener;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    private List<int> mPlayers; // Photo Player IDs

    [SerializeField]
    private GameObject glowPrePassCamera;

    void Start()
    {
        // Get components
        mSpotlight = GetComponent<Light>();
        mCamera = GetComponent<Camera>();
        mListener = GetComponent<AudioListener>();

        // Everything off by default
        mSpotlight.enabled = false;
        mCamera.enabled = false;
        mListener.enabled = false;

        // Initalize rotation information
        xRotation = transform.eulerAngles.y;
        yRotation = transform.eulerAngles.x;

        // Initialize empty player queue
        mPlayers = new List<int>();

        // Set the camera status text/color.
        UpdateCameraText();
    }
	
	void Update()
    {
        Quaternion newRotation = Quaternion.AngleAxis(xRotation, Vector3.up);
        newRotation *= Quaternion.AngleAxis(-yRotation, -Vector3.right);
        transform.localRotation = Quaternion.Slerp(transform.rotation, newRotation, Time.time * 0.01f);
    }

    /**
     * Called when the player switches to a camera.
     */
    public void Activate()
    {
        // Turn on this camera for the player.
        mCamera.enabled = true;
        mListener.enabled = true;
        glowPrePassCamera.GetComponent<Camera>().enabled = true;
        glowPrePassCamera.GetComponent<GlowPrePass>().enabled = true;
        GetComponent<GlowComposite>().enabled = true;

        // Trigger adding a player to this camera.
        photonView.RPC("AddPlayer", PhotonTargets.All, PhotonNetwork.player.ID);
    }

    /**
     * Called when a player switches away from a camera.
     */
    public void Deactivate()
    {
        // Turn off this camera for the player.
        mCamera.enabled = false;
        mListener.enabled = false;
        glowPrePassCamera.GetComponent<Camera>().enabled = false;
        glowPrePassCamera.GetComponent<GlowPrePass>().enabled = false;
        GetComponent<GlowComposite>().enabled = false;

        // Trigger removing a player from this camera.
        photonView.RPC("RemovePlayer", PhotonTargets.All, PhotonNetwork.player.ID);
    }

    /**
     * Disable preview mode. See EnablePreviewMode().
     */
    public void DisablePreviewMode()
    {
        // Turn off the camera.
        mCamera.enabled = false;

        // Reset the viewport to original dimensions.
        mCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
    }

    /**
     * Enable preview mode, which allows cameras to be viewed side-by-side
     * like a guard screen.
     */
    public void EnablePreviewMode(float size, int x, int y)
    {
        // Turn on the camera in case it was disabled, but disable sound.
        mCamera.enabled = true;
        mListener.enabled = false;

        // Set the viewport appropriately.
        mCamera.rect = new Rect(size * x, size * y, size, size);
    }

    public bool InControl()
    {
        if (mPlayers.Count == 0 || mPlayers[0] != PhotonNetwork.player.ID)
        {
            return false;
        }
        return true;
    }

    /**
     * Attempt to rotate the camera.
     */
    public void Rotate(float xRotation, float yRotation)
    {
        // Do nothing if not in control.
        if (!InControl())
        {
            return;
        }

        // Append the new rotation.
        this.xRotation += xRotation;
        this.yRotation += yRotation;

        // Limit the rotation according to these constraints.
        this.xRotation = this.xRotation % 360;
        this.yRotation = Mathf.Clamp(this.yRotation, -45, 80);

        // Notify everyone else that the camera rotated.
        photonView.RPC("UpdateRotation", PhotonTargets.Others, this.xRotation, this.yRotation);
    }

    /**
    * Set the text in the bottom-right corner, denoting the lock-status of the Camera.
    */
    void UpdateCameraText()
    {
        // First set the camera name
        cameraStatusText.text = this.gameObject.name + "\n";

        // Then set the status text
        if (mPlayers.Count == 0)
        {
            cameraStatusText.text += AVAILABLE_TEXT;
            cameraStatusText.color = AVAILABLE_COLOR;
        }
        else if (InControl())
        {
            cameraStatusText.text += IN_CONTROL_TEXT;
            cameraStatusText.color = IN_CONTROL_COLOR;
        }
        else
        {
            cameraStatusText.text += LOCKED_TEXT;
            cameraStatusText.color = LOCKED_COLOR;
        }
    }

    /*
     * Called via RPC to add a player to this camera.
     */
    [PunRPC]
    void AddPlayer(int player)
    {
        // Add the player to the queue.
        mPlayers.Add(player);

        // Because there is now a player, make sure the spotlight is on.
        mSpotlight.enabled = true;

        // Update camera text as necessary.
        UpdateCameraText();
    }

    /*
     * Called via RPC to remove a player from this camera.
     */
    [PunRPC]
    void RemovePlayer(int player)
    {
        // Remove the player from the queue.
        mPlayers.Remove(player);

        // If no more players, turn off spotlight.
        if (mPlayers.Count == 0)
        {
            mSpotlight.enabled = false;
        }

        // Update camera text as necessary.
        UpdateCameraText();
    }

    /**
     * Called via RPC to update with new rotation data.
     */
    [PunRPC]
    void UpdateRotation(float xRotation, float yRotation)
    {
        this.xRotation = xRotation;
        this.yRotation = yRotation;
    }
}
