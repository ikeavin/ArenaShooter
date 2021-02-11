using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssassin : PlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        attackSpeed = 1.2f;
        attackTimer = attackSpeed;
        health = 5f;
        maxHealth = 4f;
        speed = 4f;
        firstAbilityCooldown = 24f;
        firstAbilityTimer = 24f;
        reloadSpeed = 2.5f;
        reloadTimer = 0f;
        reloading = false;
        maxAmmo = 3;
        ammo = maxAmmo;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {

        }
    }
}
