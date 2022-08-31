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
    [SerializeField] int maxShootDistance;

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
        targetUnit.Damage();
    }

    public override string GetBaseActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

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
                if(targetUnit.IsEnemy() && unit.IsEnemy())
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onShootCompleted)
    {
        ActionStart(onShootCompleted);

        targetUnit = LevelGrid.Instance.GetAnyUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float stateAimingTimer = 1f;
        stateTimer = stateAimingTimer;

        canShootBullet = true;
    }
}
