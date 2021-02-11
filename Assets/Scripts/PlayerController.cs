using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float maxHealth;
    public float health;
    public float attackSpeed;
    public float attackTimer;
    public float firstAbilityCooldown;
    public float firstAbilityTimer;
    public float reloadSpeed;
    public float reloadTimer;
    public float maxAmmo;
    public float ammo;
    public bool reloading;
    protected float speed;
    protected float verticalInput;
    protected float horizontalInput;
    public Origin origin = Origin.Player;

    // Start is called before the first frame update
    void Start()
    {
        attackSpeed = .5f;
        attackTimer = attackSpeed;
        health = 5f;
        maxHealth = 5f;
        speed = 5f;
        firstAbilityCooldown = 24f;
        firstAbilityTimer = 24f;
        reloadSpeed = 1.5f;
        reloadTimer = 0f;
        reloading = false;
        maxAmmo = 6;
        ammo = maxAmmo;
    }

    // Update is called once per frame
    protected void Update()
    {
        attackTimer += Time.deltaTime;
        firstAbilityTimer += Time.deltaTime;

        if(reloading)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= reloadSpeed)
            {
                reloading = false;
                ammo = maxAmmo;
            }
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * speed);

        if(Input.GetMouseButtonDown(0) && attackTimer >= attackSpeed && ammo > 0)
        {
            attackTimer = 0f;
            ammo--;
            Shoot();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            reloading = true;
        }
    }

    protected void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            Vector3 direction = hit.point;
            direction -= transform.position;
            direction.y = 0;
            direction = direction.normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetDirection(direction);
            bullet.GetComponent<Bullet>().SetOrigin(this.origin);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if (bullet != null && bullet.GetOrigin() != this.origin)
        {

            this.health -= bullet.GetDamage();
            Destroy(other.gameObject);

            if(this.health <= 0)
            {
                Time.timeScale = 0f;
            }
        }
    }
}
