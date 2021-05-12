using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public float maxHealth;
    public float health;
    public float attackSpeed;
    public float attackTimer;
    public float firstAbilityCooldown;
    public float firstAbilityTimer;
    public float firstAbilityEffectTimer;
    public float firstAbilityDuration;
    public float reloadSpeed;
    public float reloadTimer;
    public float maxAmmo;
    public float ammo;
    public float defaultReloadSpeed;
    public float defaultAttackSpeed;
    public bool reloading;
    public bool abilityActivated;
    protected float speed;
    protected float verticalInput;
    protected float horizontalInput;
    public Origin origin = Origin.Player;
    private float timePerFrame = .02f;
    //public Camera tempCamera;
    //public Camera playerCamera;

    //Start is called before the first frame update
    //Initialize all base stats for the Character Class
    void Start()
    {
        attackSpeed = .5f;
        defaultAttackSpeed = .5f;
        attackTimer = attackSpeed;
        health = 5f;
        maxHealth = 5f;
        speed = 5f;
        firstAbilityCooldown = 24f;
        firstAbilityTimer = 24f;
        firstAbilityDuration = 3f;
        firstAbilityEffectTimer = 0f;
        reloadSpeed = 1.5f;
        defaultReloadSpeed = 1.5f;
        reloadTimer = 0f;
        reloading = false;
        maxAmmo = 6;
        ammo = maxAmmo;
    }

    //Update is called once per frame
    [Client]
    protected void FixedUpdate()
    {
        Debug.Log("Delta Time: " + timePerFrame);
        Debug.Log("Fixed Delta Time: " + timePerFrame);
        if (!hasAuthority) return;
        ProcessTimers();
        ProcessMovement();
        ProcessAttacks();
        ProcessAbility();
    }

    [Client]
    protected void ProcessMovement() {
        if (!hasAuthority) return;
        Debug.Log("Reached Movement");
        //Move character based on WASD input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * timePerFrame * horizontalInput * speed);
        transform.Translate(Vector3.forward * timePerFrame * verticalInput * speed);
    }

    [Client]
    protected void ProcessAttacks() {
        //If left mouse button is pressed and the player is capable of shooting, shoot
        if (!hasAuthority) return;
        Debug.Log("Reached Attacks");
        if (Input.GetMouseButton(0) && CanShoot())
        {
            attackTimer = 0f;
            ammo--;
            CmdShoot();
        }

        //If the player isn't already reloading and is has pressed R, initiate reloading
        if (Input.GetKeyDown(KeyCode.R) && reloading == false)
        {
            reloading = true;
            ammo = 0;
        }
    }

    [Client]
    protected void ProcessTimers() {
        if (!hasAuthority) return;
        Debug.Log("Reached Timers");
        //Increment attack timer
        attackTimer += timePerFrame;

        //Increment reload timer if the player is reloading
        if (reloading == true)
        {
            reloadTimer += timePerFrame;

            //If the reload timer is complete reload ammo
            if (reloadTimer >= reloadSpeed)
            {
                reloading = false;
                ammo = maxAmmo;
            }
        }
    }

    //Implement to listen for keypresses and handle timers for abilities
    protected abstract void ProcessAbility();

    //Implement to handle when the ability ends
    protected abstract void DeactivateAbility();

    //Determine if the player can currently shoot
    protected bool CanShoot() {
        if(attackTimer >= attackSpeed && ammo > 0 && reloading == false)
        {
            return true;
        }
        return false;
    }

    //Shoot a projectile based on mouse position
    [Command]
    protected void CmdShoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            //Calculate direction vector
            Vector3 direction = hit.point;
            direction -= transform.position;
            direction.y = 0;
            direction = direction.normalized;

            //Instantiate then set direction and origin
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SetDirection(direction);
            bullet.GetComponent<Bullet>().SetOrigin(this.origin);

            NetworkServer.Spawn(bullet);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {

        //Determine if the object was a bullet and was shot by a different team
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
