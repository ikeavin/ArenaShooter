using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float maxHealth;
    [SerializeField] private float weaponRange;
    [SerializeField] private float reloadTime;
    [SerializeField] private Origin origin;

    private bool avoidanceMode = false;
    private float health;
    private float timeSinceLastShot;
    private float fixedDeltaTime = .02f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;     
    }

    // Update is called once per frame
    [Server]
    void FixedUpdate()
    {
        //Find a player if there is no target
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>().gameObject;
        }

        //Shoot if possible
        timeSinceLastShot += fixedDeltaTime;
        if(Vector3.Magnitude(transform.position - player.transform.position) <= weaponRange && timeSinceLastShot >= reloadTime)
        {
            timeSinceLastShot = 0f;
            Shoot(player.transform.position, transform.position);
        }

        //Determine movement
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

    [Server]
    void Shoot(Vector3 direction, Vector3 position)
    {

        direction -= position;
        direction.y = 0;
        direction = direction.normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().SetDirection(direction);
        bullet.GetComponent<Bullet>().SetOrigin(this.origin);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if(bullet != null && bullet.GetOrigin() != this.origin)
        {
            this.health -= bullet.GetDamage();
            Destroy(other.gameObject);

            if (this.health <= 0)
            {
                Debug.Log("please kill me :(");
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
