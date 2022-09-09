using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform shootingPoint;
    [SerializeField] Transform swordTransform;
    [SerializeField] Transform rifleTransform; 
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShooting += ShootAction_OnShooting;
        }

        if(TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted; 
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        EquipRifle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("isRunning", true);
    }

    void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("isRunning", false);
    }

    void ShootAction_OnShooting(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootingPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        Vector3 targetPosition = e.targetUnit.GetWorldPosition();
        targetPosition.y = shootingPoint.position.y;
        bulletProjectile.Setup(targetPosition);
    }

    void SwordAction_OnSwordActionStarted(object sender, EventArgs e) 
    {
        EquipSword();
        animator.SetTrigger("Sword");
    }

    void SwordAction_OnSwordActionCompleted(object sender, EventArgs e) 
    {
        EquipRifle();
    }

    
    void EquipSword()
    {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    void EquipRifle()
    {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
