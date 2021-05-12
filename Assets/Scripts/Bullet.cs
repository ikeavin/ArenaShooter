using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    private float speed;
    public float range;
    public float distanceTravelled;
    public float damage;
    private Vector3 direction;
    public Origin origin;
    private float fixedDeltaTime = .02f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 50f;
        distanceTravelled = 0f;
        range = 70f;
    }

    // Update is called once per frame
    [Server]
    void FixedUpdate()
    {
        transform.Translate(direction * fixedDeltaTime * speed);
        distanceTravelled += (speed * fixedDeltaTime);
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

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return this.speed;
    }
}
