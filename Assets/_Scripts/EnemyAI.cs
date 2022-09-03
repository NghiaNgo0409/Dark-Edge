using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    State state;
    float timer;
    void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }
    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateTurnSystem;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    void SetStateTakingTurn()
    {
        timer = .5f;
        state = State.TakingTurn;
    }

    void OnTurnChanged_UpdateTurnSystem(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    bool TryTakeEnemyAIAction(Action onActionCompleted)
    {
        foreach (Unit enemy in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemy, onActionCompleted))
            {
                return true;
            }
            
        }
        return false;
    }

    bool TryTakeEnemyAIAction(Unit enemy, Action onActionCompleted)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemy.GetBaseActionArray())
        {
            if(!enemy.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue;
            }

            if(bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction =  baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValues > bestEnemyAIAction.actionValues)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if(bestEnemyAIAction != null && enemy.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onActionCompleted);
            return true;
        }
        else
        {
            return false;
        }
    }
}
