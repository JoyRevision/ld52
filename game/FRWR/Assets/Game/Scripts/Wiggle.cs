using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float bobbingAmount = 0.05f;
    public float speed = 1.0f;

    float defaultPosY = 0f;
    float defaultPosX = 0f;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
        defaultPosX = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;

        float xBob = bobbingAmount;
        float yBob = bobbingAmount;
        transform.localPosition = new Vector3(defaultPosX + Mathf.Cos(timer) * (xBob / 2), defaultPosY + Mathf.Sin(timer) * yBob, transform.localPosition.z);
    }
}
