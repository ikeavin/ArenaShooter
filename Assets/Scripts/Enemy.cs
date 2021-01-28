using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    private NavMeshAgent agent;
    public bool avoidanceMode;
    public float weaponRange;
    public float spread;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        avoidanceMode = false;
        agent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= weaponRange)
        {
            //Shoot(player.transform.position);
        }

        if(avoidanceMode == false && (transform.position - player.transform.position).magnitude < weaponRange / 2)
        {
            avoidanceMode = true;
            agent.ResetPath();
        }
        else if(avoidanceMode == true && (transform.position - player.transform.position).magnitude > weaponRange)
        {
            avoidanceMode = false;
            agent.SetDestination(player.transform.position);
        }

        if (avoidanceMode == true && agent.remainingDistance < 1)
        {
            float randomModifierX = Random.Range(-(weaponRange / 2), weaponRange / 2);
            float randomModifierY = Random.Range(-(weaponRange / 2), weaponRange / 2);
            Vector3 randomPosition = transform.position;
            randomPosition.x += randomModifierX;
            randomPosition.y += randomModifierY;
            agent.SetDestination(randomPosition);
        }
    }

    void Shoot(Vector3 destination)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if(bullet != null)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
