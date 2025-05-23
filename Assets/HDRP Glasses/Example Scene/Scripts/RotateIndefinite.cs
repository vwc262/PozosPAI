using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIndefinite : MonoBehaviour
{
    float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if (time < 20)
        {
            transform.Rotate(Vector3.up, 0.05f);
        }
        if (time > 20)
        {
            transform.Rotate(-Vector3.up, 0.05f);
        }
        if (time > 50)
        {
            time = 0;
        }
    }
}
