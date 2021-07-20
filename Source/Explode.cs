using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float radius;
    public float power;

    void Start()
    {
        Invoke("Explosion", 0.01f);
    }

    void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D hit in colliders)
        {
            if (hit.GetComponent<Rigidbody2D>() != null)
            {
                float force = radius - Vector3.Distance(hit.transform.position, explosionPos);

                hit.GetComponent<Rigidbody2D>().AddForce(power * 10f * force * ((hit.transform.position - explosionPos).normalized));
            }
        }

        PlaySound();
    }

    void PlaySound(){
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if(onScreen){
            GetComponent<AudioSource>().Play();
        }
    }

}
