using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 direction;
    public float force = 1250f;

    public bool active = true;

    public int damage = 1;

    float timer;

    AudioSource hitSound;

    void Start()
    {
        // Ignore all collisions with the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponentInChildren<BoxCollider>(), GetComponent<BoxCollider>());

        hitSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // After 15 secs just destroy
        if (timer > 15f)
        {
            Destroy(gameObject);
        }
    }

    // called from the attacker when instantiating
    public void Setup(Vector3 dir)
    {
        direction = dir;

        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!active) return;

        var tag = other.gameObject.tag;
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<CarrotController>().DoDamage(damage);
            hitSound.Play();
        }


        active = false;
    }
}
