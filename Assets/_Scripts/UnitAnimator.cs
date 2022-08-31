using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform bulletProjectilePrefab;
    [SerializeField] Transform shootingPoint;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if(TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShooting += ShootAction_OnShooting;
        }
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
}
