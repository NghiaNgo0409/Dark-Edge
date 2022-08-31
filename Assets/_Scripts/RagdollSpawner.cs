using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour
{
    [SerializeField] Transform ragdollPrefab;
    [SerializeField] Transform originalRootBone; 
    HealthSystem healthSystem;
    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform unitRagdollPrefab = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = unitRagdollPrefab.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
