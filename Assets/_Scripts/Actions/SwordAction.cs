using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    enum State
    {
        SwingSwordBeforeHit,
        SwingSwordAfterHit
    }
    public static event EventHandler OnAnySwordAction;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    [SerializeField] int maxSwordDistance;
    [SerializeField] float rotateSpeed;

    State state;
    float stateTimer;

    Unit targetUnit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingSwordBeforeHit:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
                break;
            case State.SwingSwordAfterHit:
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
            case State.SwingSwordBeforeHit:
                if (stateTimer <= 0f)
                {
                    state = State.SwingSwordAfterHit;
                    float afterHitStateTimer = .1f;
                    stateTimer = afterHitStateTimer;
                    targetUnit.Damage(100);
                    OnAnySwordAction?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.SwingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetBaseActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValues = 50,
            actionName = "Sword Action"
        };
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
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
        state = State.SwingSwordBeforeHit;
        float beforeHitStateTimer = .5f;
        stateTimer = beforeHitStateTimer;
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetMaxDistance()
    {
        return maxSwordDistance;
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPosition().Count;
    }
}
