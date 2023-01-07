using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CarrotController : MonoBehaviour
{
    public GameObject mesh;
    private GameObject player;

    public float rotationSpeed = 10f;
    public float moveSpeed = 5f;

    private CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        mesh.transform.LookAt(player.transform, Vector3.up);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed;

        _controller.Move(move * Time.deltaTime);
    }
}
