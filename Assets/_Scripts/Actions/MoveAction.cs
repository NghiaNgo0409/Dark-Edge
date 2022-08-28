using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    Vector3 targetPosition;
    float stoppingDistance = .1f;
    float rotateSpeed = 10f;
    
    Animator unitAnim;
    [SerializeField] int maxMoveDistance;

    public Action onMoveCompleted;
    protected override void Awake() 
    {
        base.Awake();
        unitAnim = GetComponentInChildren<Animator>();
        targetPosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive) return;

        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            unitAnim.SetBool("isRunning", true);
            Vector3 desiredDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 10f;
            transform.position += desiredDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, desiredDirection, rotateSpeed * Time.deltaTime);
        }
        else
        {
            unitAnim.SetBool("isRunning", false);
            isActive = false;
            onMoveCompleted();
        }
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPosition();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPosition()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
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

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public void Move(GridPosition gridPosition, Action onMoveCompleted)
    {
        this.onMoveCompleted = onMoveCompleted;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public override string GetBaseActionName()
    {
        return "Move";
    }
}
