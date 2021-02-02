using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    public float range;
    public float distanceTravelled;
    public float damage;
    private Vector3 direction;
    public Origin origin;

    // Start is called before the first frame update
    void Start()
    {
        speed = 20f;
        distanceTravelled = 0f;
        range = 50f;
        damage = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);

        distanceTravelled += (speed * Time.deltaTime);
        if(distanceTravelled >= range)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public Vector3 GetDirection()
    {
        return this.direction;
    }

    public void SetOrigin(Origin origin)
    {
        this.origin = origin;
    }

    public Origin GetOrigin()
    {
        return this.origin;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetDamage()
    {
        return this.damage;
    }
}
