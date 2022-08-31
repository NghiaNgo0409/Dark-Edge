using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const int ACTION_MAX_POINTS = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    GridPosition gridPosition;
    HealthSystem healthSystem;
    MoveAction moveAction;
    SpinAction spinAction;
    BaseAction[] baseActionArray;
    int actionPoints = ACTION_MAX_POINTS;

    [SerializeField] bool isEnemy;
    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        healthSystem.OnDead += HealthSystem_OnDead;

        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateSystem;
    }

    // Update is called once per frame
    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPointsToTakeAction(baseAction.GetActionPoints());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPoints();
    }

    public void SpendActionPointsToTakeAction(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnTurnChanged_UpdateSystem(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_MAX_POINTS;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

    public void Damage()
    {
        healthSystem.Damage(40);
    }
    
}
