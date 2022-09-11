using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Aiming,
    Shooting,
    Cooloff
}
public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShootActions;
    public event EventHandler<OnShootEventArgs> OnShooting;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    [SerializeField] int maxShootDistance;
    [SerializeField] LayerMask obstacleLayer;

    State state;
    float stateTimer;

    Unit targetUnit;
    bool canShootBullet;
    [SerializeField] float rotateSpeed;
    
    public Action onShootCompleted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if(stateTimer <= 0f)
        {
            NextState();
        }
    }

    void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                if(stateTimer <= 0f)
                {
                    state = State.Shooting;
                    float stateShootingTimer = .1f;
                    stateTimer = stateShootingTimer;
                }
                break;
            case State.Shooting:
                if(stateTimer <= 0f)
                {
                    state = State.Cooloff;
                    float stateCooloffTimer = .5f;
                    stateTimer = stateCooloffTimer;
                }
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    void Shoot()
    {
        OnAnyShootActions?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

        OnShooting?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(40);
    }

    public override string GetBaseActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPosition(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPosition(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for(int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance)
                {
                    // Make sure the shooting range is a curve
                    continue;
                }
                // Do not shoot the grid position which has not unit
                if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetAnyUnitAtGridPosition(testGridPosition);
                //Do not shoot the grid position when the units are in the same team
                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float unitShoulderHeight = 2.4f;
                if(Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstacleLayer))
                {
                    //Blocked by an Obstacle
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onShootCompleted)
    {
        targetUnit = LevelGrid.Instance.GetAnyUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float stateAimingTimer = 1f;
        stateTimer = stateAimingTimer;

        canShootBullet = true;

        ActionStart(onShootCompleted);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetAnyUnitAtGridPosition(gridPosition);

        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValues = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
            actionName = "Shoot Action"
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPosition(gridPosition).Count;
    }
}
