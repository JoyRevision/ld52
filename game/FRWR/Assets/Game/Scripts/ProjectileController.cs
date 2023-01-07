using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 direction;
    public float speed = 0.75f;

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponentInChildren<BoxCollider>(), GetComponentInChildren<BoxCollider>());

        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * 2500f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
