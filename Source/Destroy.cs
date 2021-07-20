using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float delay;
    void Start()
    {
        Destroy(this.gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
