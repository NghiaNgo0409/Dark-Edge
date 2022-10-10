using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip akSFX, zombieEatingSFX, meleeSFX, zombieHurtSFX, unitHurtSFX, explosionSFX, interactSFX;
    private void OnEnable()
    {
        ShootAction.OnAnyShootActions += ShootAction_OnAnyShootActions;
        ZombieAttackAction.OnAnyZombieAttack += ZombieAttackAction_OnAnyZombieAttack;
        HealthSystem.OnStaticDamage += HealthSystem_OnStaticDamage;
        GrenadeAction.OnAnyThrowGrenadeActions += GrenadeAction_OnAnyThrowGrenadeActions;
        InteractAction.OnAnyInteractActions += InteractAction_OnAnyInteractActions;
        SwordAction.OnAnySwordAction += SwordAction_OnAnySwordAction;
    }

    private void OnDisable()
    {
        ShootAction.OnAnyShootActions -= ShootAction_OnAnyShootActions;
        ZombieAttackAction.OnAnyZombieAttack -= ZombieAttackAction_OnAnyZombieAttack;
        HealthSystem.OnStaticDamage -= HealthSystem_OnStaticDamage;
        GrenadeAction.OnAnyThrowGrenadeActions -= GrenadeAction_OnAnyThrowGrenadeActions;
        InteractAction.OnAnyInteractActions -= InteractAction_OnAnyInteractActions;
        SwordAction.OnAnySwordAction -= SwordAction_OnAnySwordAction;
    }
    // Start is called before the first frame update
    void Start()
    {
        //ShootAction.OnAnyShootActions += ShootAction_OnAnyShootActions;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaySFX(AudioClip sfx)
    {   
        sfxSource.PlayOneShot(sfx);
    }

    void ShootAction_OnAnyShootActions(object sender, EventArgs e)
    {
        PlaySFX(akSFX);
    }

    void ZombieAttackAction_OnAnyZombieAttack(object sender, EventArgs e)
    {
        PlaySFX(zombieEatingSFX);
    }

    void HealthSystem_OnStaticDamage(object sender, EventArgs e)
    {
        HealthSystem healthSystem = sender as HealthSystem;
        Unit unit = healthSystem.GetComponent<Unit>();
        if (unit.IsEnemy())
        {
            PlaySFX(zombieHurtSFX);
        }
        else
        {
            PlaySFX(unitHurtSFX);
        }
    }

    void GrenadeAction_OnAnyThrowGrenadeActions(object sender, EventArgs e)
    {
        PlaySFX(explosionSFX);
    }

    void InteractAction_OnAnyInteractActions(object sender, EventArgs e)
    {
        PlaySFX(interactSFX);
    }

    void SwordAction_OnAnySwordAction(object sender, EventArgs e)
    {
        PlaySFX(meleeSFX);
    }
}
