using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject bullet;
    public float wait;
    public bool tracking;
    GameObject m;

    void Start()
    {
        Invoke("Spawn", wait);
    }

    void Spawn()
    {
        Instantiate(bullet, transform.position + transform.up * 1.9f, transform.rotation);
        Invoke("Spawn", wait);
    }

    void Update(){ 
        if(tracking){
            Track();
        } 
    }

    void Track(){
        float maxDistance = 5000f;

        foreach(Marble ma in FindObjectsOfType<Marble>()){
            if((transform.position - ma.transform.position).sqrMagnitude < maxDistance){
                maxDistance = (transform.position - ma.transform.position).sqrMagnitude;
                m = ma.gameObject;
            }
        }

        if(m != null){
            Vector3 dir =  m.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        m = null;
    }

}
