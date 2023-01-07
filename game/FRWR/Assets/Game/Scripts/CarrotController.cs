using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
    public GameObject mesh;

    private GameObject player;

    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        mesh.transform.LookAt(player.transform, Vector3.up);
    }
}
