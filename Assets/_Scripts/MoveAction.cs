using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    Vector3 targetPosition;
    float stoppingDistance = .1f;
    float rotateSpeed = 10f;
    
    Animator unitAnim;
    Unit unit;
    [SerializeField] int maxMoveDistance;
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
        unitAnim = GetComponentInChildren<Animator>();
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
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
                Debug.Log(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
