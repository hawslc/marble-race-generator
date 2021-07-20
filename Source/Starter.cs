using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("space"))
        {
            foreach(Marble m in FindObjectsOfType<Marble>())
            {
                m.GetComponent<TrailRenderer>().enabled = true;
            }
            FindObjectOfType<GameMaster>().time = 3;
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
