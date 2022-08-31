using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    [SerializeField] int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        health -= amount;
        
        if(health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
