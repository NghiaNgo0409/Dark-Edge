using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;
    public static event EventHandler OnStaticDamage;

    [SerializeField] int health;
    [SerializeField] Transform ragdollRootBone;
    int healthMax;
    // Start is called before the first frame update
    void Start()
    {
        healthMax = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        health -= amount;

        OnDamage?.Invoke(this, EventArgs.Empty);
        OnStaticDamage?.Invoke(this, EventArgs.Empty);


        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
