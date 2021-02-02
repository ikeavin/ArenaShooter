using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    private NavMeshAgent agent;
    public bool avoidanceMode;
    public float maxHealth;
    public float health;
    public float weaponRange;
    public float spread;
    public float timeSinceLastShot;
    public float reloadTime;
    public Origin origin;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        avoidanceMode = false;
        agent.SetDestination(player.transform.position);
        reloadTime = 0.75f;
        timeSinceLastShot = reloadTime;
        this.origin = Origin.Enemy;
        maxHealth = 2f;
        health = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if(Vector3.Magnitude(transform.position - player.transform.position) <= weaponRange && timeSinceLastShot >= reloadTime)
        {
            timeSinceLastShot = 0f;
            Shoot(player.transform.position, transform.position);
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

    void Shoot(Vector3 direction, Vector3 position)
    {
        
        direction.y = position.y;
        direction -= position;
        direction = direction.normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.GetComponent<Bullet>().SetDirection(direction);
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        bullet.GetComponent<Bullet>().SetOrigin(this.origin);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if(bullet != null && bullet.GetOrigin() != this.origin)
        {
            this.health -= bullet.GetDamage();
            Destroy(collision.gameObject);

            if (this.health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
