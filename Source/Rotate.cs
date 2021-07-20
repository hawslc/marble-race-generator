using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float spinSpeed;

    void FixedUpdate()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.fixedDeltaTime);
    }
}
