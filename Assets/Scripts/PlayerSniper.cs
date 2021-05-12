using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSniper : PlayerController
{
    protected bool zoom;
    protected float cameraMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        attackSpeed = 1.2f;
        attackTimer = attackSpeed;
        health = 5f;
        maxHealth = 4f;
        speed = 4f;
        firstAbilityCooldown = 1f;
        firstAbilityTimer = -1f;
        reloadSpeed = 2.5f;
        reloadTimer = 0f;
        reloading = false;
        maxAmmo = 3;
        ammo = maxAmmo;
        zoom = false;
        //cameraMagnitude = playerCamera.transform.position.magnitude;
    }

    protected override void DeactivateAbility()
    {

    }

    protected override void ProcessAbility()
    {
        /**
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zoom = !zoom;
            firstAbilityTimer = 0f;
        }

        if (zoom == true)
        {
            firstAbilityTimer += Time.deltaTime;
            if (firstAbilityTimer < firstAbilityCooldown)
            {
                playerCamera.transform.Translate(Time.deltaTime * (new Vector3(0, 1, -3)) * 4);
            }
        }
        else {
            firstAbilityTimer -= Time.deltaTime;
            if (firstAbilityTimer > -firstAbilityCooldown)
            {
                playerCamera.transform.Translate(Time.deltaTime * (new Vector3(0, -1, 3)) * 4);
            }
        }
        **/
    }
}
