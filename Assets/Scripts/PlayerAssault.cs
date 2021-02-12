using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAssault : PlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        attackSpeed = .5f;
        defaultAttackSpeed = .5f;
        attackTimer = attackSpeed;
        health = 6f;
        maxHealth = 6f;
        speed = 5f;
        firstAbilityCooldown = 24f;
        firstAbilityTimer = 24f;
        firstAbilityDuration = 3f;
        firstAbilityEffectTimer = 0f;
        reloadSpeed = 1.5f;
        defaultReloadSpeed = 1.5f;
        reloadTimer = 0f;
        reloading = false;
        maxAmmo = 8;
        ammo = maxAmmo;
    }
    
    protected override void ProcessAbility()
    {
        //Determine if ability can be activated
        if (Input.GetKeyDown(KeyCode.Space) && firstAbilityTimer >= firstAbilityCooldown)
        {
            firstAbilityTimer = 0;
            firstAbilityEffectTimer = 0;
            abilityActivated = true;
        }

        //Increment cooldown timers
        if (abilityActivated)
        {
            firstAbilityEffectTimer += Time.deltaTime;
            if (firstAbilityEffectTimer > firstAbilityDuration)
            {
                abilityActivated = false;
                DeactivateAbility();
            }
        }
        else
        {
            firstAbilityTimer += Time.deltaTime;
        }


        if (abilityActivated)
        {
            reloadSpeed = .1f;
            attackSpeed = .1f;
            ammo = 10;
        }
    }

    protected override void DeactivateAbility()
    {
        reloadSpeed = defaultReloadSpeed;
        attackSpeed = defaultAttackSpeed;
        ammo = maxAmmo;
    }
}
