using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictMovement : MonoBehaviour
{
    public float xLimit;
    public float yLimit;

    public float z;

    void Update()
    {
        if(transform.position.x > xLimit)
        {
            transform.position = new Vector3(xLimit, transform.position.y, z);
        } 
        if(transform.position.x < -xLimit)
        {
            transform.position = new Vector3(-xLimit, transform.position.y, z);
        }
        if(transform.position.y > yLimit)
        {
            transform.position = new Vector3(transform.position.x, yLimit, z);
        }
        if(transform.position.y < -yLimit)
        {
            transform.position = new Vector3(transform.position.x, -yLimit, z);
        }
        
    }
}
