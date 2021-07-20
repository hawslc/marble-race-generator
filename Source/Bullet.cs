using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public int life;
    public float speed;
    public bool burst;
    public int burstAmount;
    public bool spread3;
    public bool spread5;
    public bool spread9;
    public bool spread15;
    public bool shooting;
    public float burstDelay;
    public GameObject bullet;
    public Transform me;
    int shootTimer;
    public GameObject explosionEffect;
    

    void Start()
    {
        shootTimer = Random.Range(25, 45);
        if (burst)
        {
            StartCoroutine(Burst());
        }
        if (spread3)
        {
            Spread3();
        }
        if (spread5)
        {
            Spread5();
        }
        if (spread9)
        {
            Spread9();
        }
        if (spread15)
        {
            Spread15();
        }

    }

    void Update()
    {
        if (shooting)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        transform.position += transform.up * Time.deltaTime * speed;

        life--;
        if (life <= 0)
        {
            Die();
        }
    }

    void Shoot()
    {
        shootTimer--;
         
        if(shootTimer <= 0)
        {
            shootTimer = Random.Range(25, 45);
            ShootBullet();
        }
    }

    public void Die()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    void ShootBullet()
    {

        GameObject shot = Instantiate(bullet, transform.position + transform.right.normalized * -0.4f + transform.up.normalized * 0.2f, transform.rotation);
        Transform bulletTransform = shot.GetComponent<Transform>();
        bulletTransform.Rotate(0f, 0f, 90f, Space.Self);
        shot = Instantiate(bullet, transform.position + transform.right.normalized * 0.4f + transform.up.normalized * 0.2f, transform.rotation);
        bulletTransform = shot.GetComponent<Transform>();
        bulletTransform.Rotate(0f, 0f, -90f, Space.Self);
    }

    IEnumerator Burst()
    {
        yield return new WaitForSeconds(burstDelay);

        //transform.rotation = Quaternion.identity;
        for (int i = 0; i < burstAmount; i++)
        {
            GameObject shot = Instantiate(bullet, transform.position + transform.up.normalized * 0.1f, transform.rotation);
            //float f = (float)i * 45f;
            transform.Rotate(0f, 0f, (360f / (float)burstAmount), Space.Self);
            Transform bulletTransform = shot.GetComponent<Transform>();
            bulletTransform.rotation = transform.rotation;
            //transform.rotation = Quaternion.identity;
        }

        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D()
    {
        Die();
    }

    void Spread3()
    {
        float change = 22.5f; // angle of degrees difference between each bullet

        for (int i = -1; i < 2; i++)
        {
            Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, change * i));
        }

        Destroy(this.gameObject);
    }

    void Spread5()
    {
        float change = 9f; // angle of degrees difference between each bullet

        for (int i = -2; i < 3; i++)
        {
            Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, change * i));
        }

        Destroy(this.gameObject);
    }

    void Spread9()
    {
        float change = 5f; // angle of degrees difference between each bullet

        for (int i = -4; i < 5; i++)
        {
            Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, change * i));
        }

        Destroy(this.gameObject);
    }

    void Spread15()
    {
        float change = 3f; // angle of degrees difference between each bullet

        for (int i = -7; i < 8; i++)
        {
            Instantiate(bullet, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, change * i));
        }

        Destroy(this.gameObject);
    }
}
