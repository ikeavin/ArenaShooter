using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    public float range;
    public float distanceTravelled;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
        distanceTravelled = 0f;
        range = 50f;
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
        return direction;
    }

}
