using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour {

    public PlayerController player;
    public GameObject explosionPrefab;
    private float ROCKET_SPEED = 12.0f;
    private float ROCKET_LIFETIME = 5.0f;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.transform.position;
        Vector2 rocketDir = (mousePos - playerPos).normalized;
        float angle = Mathf.Atan2(rocketDir.y, rocketDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
        Rigidbody2D rocketBody = GetComponent<Rigidbody2D>();
        Vector2 force = rocketDir * ROCKET_SPEED;
        rocketBody.AddForce(force, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            return;
        
        Vector3 explosionPos = collision.contacts[0].point;
        explosionPos.z = 0;
        GameObject explosion = Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
        explosion.GetComponent<Explosion>().player = player;
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(ROCKET_LIFETIME);
        Destroy(gameObject);
        yield return null;
    }
}
