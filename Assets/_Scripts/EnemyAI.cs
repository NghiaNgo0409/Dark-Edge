using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateTurnSystem;
    }

    // Update is called once per frame
    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn()) return;

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }

    void OnTurnChanged_UpdateTurnSystem(object sender, EventArgs e)
    {
        timer = 2f;
    }
}
