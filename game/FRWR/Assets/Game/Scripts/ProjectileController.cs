using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 direction;
    public float speed = 0.75f;

    public bool active = true;

    public int damage = 1;

    // called from the attacker when instantiating
    public void Setup(Vector3 dir)
    {
        direction = dir;

        // Ignore all collisions with the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponentInChildren<BoxCollider>(), GetComponent<BoxCollider>());

        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * 2500f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!active) return;
        if (other.gameObject.tag == "Player") return;

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<CarrotController>().DoDamage(damage);
        }

        active = false;
    }
}
