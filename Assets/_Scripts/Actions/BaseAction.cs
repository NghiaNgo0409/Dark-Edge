using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionCompleted;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract string GetBaseActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPosition();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPosition();

    public virtual int GetActionPoints()
    {
        return 1;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    protected void ActionStart(Action onActionCompleted)
    {
        this.onActionCompleted = onActionCompleted;
        isActive = true;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionCompleted();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }
}
