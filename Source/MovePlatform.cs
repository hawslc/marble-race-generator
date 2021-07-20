using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public float speed;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        rb.MovePosition((Vector2)Vector3.Lerp(start.position, end.position, time));
    }


    
}
