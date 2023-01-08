using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinAmt = 75f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, spinAmt * Time.deltaTime, 0), Space.Self);
    }
}
