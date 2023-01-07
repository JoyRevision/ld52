using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{

    private CharacterController _controller;

    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    public GameObject mesh;

    private float _gravity = 9.8f;
    private float _verticalSpeed = 0f;

    private bool _isMoving = false;

    public Camera cameraMain;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void LookAtCursor()
    {
        // must get a resolution modifier since we render to a texture :)
        Vector2 renderTextureRes = new Vector2(320f, 240f);
        Vector2 outputRes = new Vector2(640f, 480f);
        Vector2 resMod = new Vector2(outputRes.x / renderTextureRes.x, outputRes.y / renderTextureRes.y);

        var mousePos = Input.mousePosition;
        var screenPoint = new Vector3(mousePos.x / resMod.x, mousePos.y / resMod.y, mousePos.z);
        Ray mouseRay = cameraMain.ScreenPointToRay(screenPoint);
        Plane p = new Plane(Vector3.up, mesh.transform.position);

        if (p.Raycast(mouseRay, out float hitDist))
        {
            Vector3 hitPoint = mouseRay.GetPoint(hitDist);
            mesh.transform.LookAt(hitPoint);

            Debug.DrawLine(mesh.transform.position, hitPoint, Color.magenta);
        }
    }

    // void LateUpdate()
    // {
    //     LookAtCursor();
    // }

    // Update is called once per frame
    void Update()
    {
        // TODO: Make this smooth / lerp it up
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed;


        LookAtCursor();

        // Rotate towards direction of movement
        if (move != Vector3.zero)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        // Gravity
        if (_controller.isGrounded)
        {
            _verticalSpeed = 0;
        }
        _verticalSpeed -= _gravity * Time.deltaTime;
        move.y = _verticalSpeed;

        _controller.Move(move * Time.deltaTime);
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public void Enable()
    {
        enabled = true;
        _controller.enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        _controller.enabled = false;
    }
}
