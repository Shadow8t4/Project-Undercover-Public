using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spy : Photon.PunBehaviour {

    private NavMeshAgent agent;
    public Animation shakeAnimation;
    public bool isShaking = false;
    public static HashSet<Spy> shakingSpies;
    public GameObject nameTag;

    void Start()
    {
        if (shakingSpies == null)
            shakingSpies = new HashSet<Spy>();
        agent = GetComponent<NavMeshAgent>();
        transform.Find("Body").GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f);
        if (!GetComponent<PlayerController>().enabled && PersistantPlayerSettings.character == PersistantPlayerSettings.Character.Spy)
        {
            nameTag.SetActive(true);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (SucessfulShakeCheck())
            {
                var manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                manager.CompleteMission();
            }
        }
        if (nameTag.activeInHierarchy)
        {
            nameTag.transform.LookAt(2 * nameTag.transform.position - Camera.main.transform.position);
        }
    }

    bool SucessfulShakeCheck()
    {
        if (shakingSpies.Count > 1)
        {
            foreach (var spy1 in shakingSpies)
            {
                foreach (var spy2 in shakingSpies)
                {
                    if (spy1.GetInstanceID() == spy2.GetInstanceID())
                        continue;
                    if (!spy1.isShaking || !spy2.isShaking)
                        continue;
                    float mag = (spy1.transform.position - spy2.transform.position).sqrMagnitude;
                    if (mag > 1.5)
                        continue;
                    float angle = Vector3.Angle(spy1.transform.forward, spy2.transform.forward);
                    if (angle < 140.0f)
                        continue;
                    return true;
                }
            }
        }
        return false;
    }

    public void UpdateTarget(Vector3 target)
    {
        photonView.RPC("UpdateTargetRPC", PhotonTargets.All, target);
    }

    [PunRPC]
    void UpdateTargetRPC(Vector3 target)
    {
        agent.destination = target;
    }

    public void SetColor()
    {
        photonView.RPC("SetColorRPC", PhotonTargets.All, new Vector3(Random.value, Random.value, Random.value));
    }

    [PunRPC]
    void SetColorRPC(Vector3 color)
    {
        transform.Find("Body").GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z);
    }

    public void HandShake()
    {
        if (!isShaking)
            photonView.RPC("HandshakeRPC", PhotonTargets.All);
    }

    IEnumerator FinishShake()
    {
        yield return new WaitForSeconds(0.5f);
        isShaking = false;
        shakingSpies.Remove(this);
    }

    [PunRPC]
    void HandshakeRPC()
    {
        isShaking = true;
        shakingSpies.Add(this);
        shakeAnimation.Play();
        StartCoroutine(FinishShake());
    }
}
