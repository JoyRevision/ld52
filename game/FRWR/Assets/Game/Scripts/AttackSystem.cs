using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public Texture2D cursorTex;
    public CursorMode cursorMode = CursorMode.Auto;

    public Camera cameraMain;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTex, Vector2.zero, cursorMode);
    }

    Vector3 GetAttackLocation()
    {
        var mousePos = Input.mousePosition;
        var screenPoint = new Vector3(mousePos.x, mousePos.y, mousePos.z);
        Ray mouseRay = cameraMain.ScreenPointToRay(screenPoint);
        Plane p = new Plane(Vector3.up, transform.position);

        if (p.Raycast(mouseRay, out float hitDist))
        {
            Vector3 hitPoint = mouseRay.GetPoint(hitDist);
            return hitPoint;
        }

        return Vector3.zero;
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            var attackPos = GetAttackLocation();
            Vector3 direction = (attackPos - transform.position).normalized;

            // spawn a trowel and give it the starting point (transform.position) and the direction
            Debug.Log("Attacking in: " + direction);

            GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
            p.GetComponent<ProjectileController>().SetDirection(direction);
        }
    }
}