using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    public Texture2D cursorTex;
    public CursorMode cursorMode = CursorMode.Auto;

    public Camera cameraMain;

    public GameObject projectile;

    const float COOLDOWNSEC = 0.25f;
    float cooldownTimer = COOLDOWNSEC;

    const float POWERUPTIME = 2.5f;
    float poweredUpTime = POWERUPTIME;

    // For shot delay
    const float POWERUPDELAY = 0.1f;
    float powerUpDelayTimer = POWERUPDELAY;
    bool poweredUp = false;

    public void TriggerPowerUp()
    {
        poweredUpTime = 0f;
    }

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
        cooldownTimer += Time.deltaTime;
        powerUpDelayTimer += Time.deltaTime;
        poweredUpTime += Time.deltaTime;

        if (poweredUpTime < POWERUPTIME)
            poweredUp = true;
        else
            poweredUp = false;

        if (poweredUp && powerUpDelayTimer >= POWERUPDELAY)
        {
            var attackPos = GetAttackLocation();
            Vector3 direction = transform.forward.normalized;

            // spawn a trowel/projectile and give it the starting point (transform.position) and the direction
            GameObject p = Instantiate(projectile, transform.position, transform.rotation);
            p.GetComponent<ProjectileController>().Setup(direction);

            powerUpDelayTimer = 0f;
        }
        else if (Input.GetButtonDown("Attack") && cooldownTimer > COOLDOWNSEC)
        {
            var attackPos = GetAttackLocation();
            Vector3 direction = transform.forward.normalized;

            // spawn a trowel/projectile and give it the starting point (transform.position) and the direction
            GameObject p = Instantiate(projectile, transform.position, transform.rotation);
            p.GetComponent<ProjectileController>().Setup(direction);

            cooldownTimer = 0f;
        }
    }
}
