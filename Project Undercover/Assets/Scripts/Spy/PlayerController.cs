using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public GameObject particle;
    public GameObject cameraRigPrefab;
    private GameObject cameraRig;
    public Transform cameraTarget;
    private Vector3 target;
    private Spy spy;
    private int mask;

	void Start() 
	{
        int layerMask = LayerMask.NameToLayer("Floor");
        mask = 1 << layerMask;
        spy = GetComponent<Spy>();
        cameraRig = Instantiate(cameraRigPrefab, Vector3.zero, Quaternion.identity);
        cameraRig.GetComponentInChildren<ThirdPersonCameraController>().SetTarget(cameraTarget);
	}

	void Update()
	{
		RaycastHit hit;
		if (Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 100.0f, mask)) {
				Vector3 wantedPos = hit.point;
                Instantiate(particle, wantedPos, Quaternion.Euler(-90,0,0));
                wantedPos.y = 0.5f;
                spy.UpdateTarget(wantedPos);
			}
		}

        if (Input.GetKeyDown("space"))
        {
            spy.HandShake();
        }
	}
}