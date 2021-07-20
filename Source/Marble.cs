using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "UpsideDown")
        {
            rb.gravityScale = -1;
        } else if(c.gameObject.tag == "RightSideUp")
        {
            rb.gravityScale = 1;
        }
    }
}
