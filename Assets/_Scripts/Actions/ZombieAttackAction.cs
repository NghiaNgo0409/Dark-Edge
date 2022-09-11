using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackAction : BaseAction
{
    enum State
    {
        BeforeScreaming,
        AfterAttacking
    }
    [SerializeField] int maxZombieAttackDistance;
    [SerializeField] float rotateSpeed;

    public static event EventHandler OnAnyZombieAttack;
    public event EventHandler OnZombieAttackStarted;
    public event EventHandler OnZombieAttackCompleted;

    State state;
    float stateTimer;

    Unit targetUnit;
    // Start is called before the first frame update
    void Start()
    {
        state = State.BeforeScreaming;
    }

    // Update is called once per frame
    void Update()
    {
         if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.BeforeScreaming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
                break;
            case State.AfterAttacking:
                ;
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    void NextState()
    {
        switch (state)
        {
            case State.BeforeScreaming:
                if (stateTimer <= 0f)
                {
                    state = State.AfterAttacking;
                    float afterHitStateTimer = 3f;
                    stateTimer = afterHitStateTimer;
                    targetUnit.Damage(40);
                    OnAnyZombieAttack?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.AfterAttacking:
                OnZombieAttackCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetBaseActionName()
    {
        return "ZombieAttack";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValues = 200,
            actionName = "ZombieAttack"
        };
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxZombieAttackDistance; x <= maxZombieAttackDistance; x++)
        {
            for (int z = -maxZombieAttackDistance; z <= maxZombieAttackDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                // Do not shoot the grid position which has not unit
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetAnyUnitAtGridPosition(testGridPosition);
                //Do not shoot the grid position when the units are in the same team
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetAnyUnitAtGridPosition(gridPosition);
        state = State.BeforeScreaming;
        float beforeHitStateTimer = 4f;
        stateTimer = beforeHitStateTimer;
        OnZombieAttackStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPosition().Count;
    }
}
