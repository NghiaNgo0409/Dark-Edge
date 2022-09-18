using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    List<Vector3> positionList;
    int currentPositionIndex;
    float stoppingDistance = .1f;
    float rotateSpeed = 10f;
    static int maxMoveDistance = 7;

    public Action onMoveCompleted;
    protected override void Awake() 
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 desiredDirection = (targetPosition - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, desiredDirection, rotateSpeed * Time.deltaTime);
            float moveSpeed = 10f;
            transform.position += desiredDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }   
        }
    }

    List<GridPosition> validGridPositionList = new List<GridPosition>(8);
    public override List<GridPosition> GetValidActionGridPosition()
    {
        // List<GridPosition> validGridPositionList = new List<GridPosition>();
        validGridPositionList.Clear();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                // Do not move to the grid position which unit is staying there
                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }
                // Do not move to the grid position which has unit already
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {   
                    continue;
                }

                if(!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathfindingDistanceMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    //Path length is too long
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    List<GridPosition> pathGridPositionList = new List<GridPosition>(1000);
    public override void TakeAction(GridPosition gridPosition, Action onMoveCompleted)
    {
        // List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
        int pathLength = 0;
        var findPathTask = Task.WhenAny(Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, pathLength));
        pathGridPositionList = findPathTask.Result.Result.Value.GridPositions;
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onMoveCompleted);
    }

    public override string GetBaseActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ZombieAttackAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValues = targetCountAtGridPosition * 10,
            actionName = "Move Action"
        };
    }

    public static int GetMaxMoveDistance()
    {
        return maxMoveDistance;
    }
}
