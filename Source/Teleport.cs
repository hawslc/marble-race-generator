using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform end;

    void OnTriggerEnter2D(Collider2D c)
    {
        c.gameObject.transform.position = end.position;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        c.collider.gameObject.transform.position = end.position;
    }
}
