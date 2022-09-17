using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnEnemyAI : MonoBehaviour
{
    [SerializeField] EnemyAI enemyAI;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Go in");
            enemyAI.SetIsAI(true);
        }
    }
}
