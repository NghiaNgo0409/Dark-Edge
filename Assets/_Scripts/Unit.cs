using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //const int ACTION_MAX_POINTS = 100;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    public static event EventHandler OnAnyUnitPickUpItems;

    GridPosition gridPosition;
    HealthSystem healthSystem; 
    BaseAction[] baseActionArray;
    int actionPoints;

    [SerializeField] bool isEnemy;
    [SerializeField] static bool hasGun;
    [SerializeField] static bool hasMelee = true;
   
    [SerializeField] Transform bloodSplashVFXPrefab;
    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
        SetActionPoints();
    }
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        if (DoesHasGun() && !IsEnemy()) GetComponent<ShootAction>().enabled = true;
        if (DoesHasMelee() && !IsEnemy()) GetComponent<SwordAction>().enabled = true;

        healthSystem.OnDead += HealthSystem_OnDead;

        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateSystem;
        
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            // Unit changed Grid Position
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    void SetActionPoints()
    {
        if (isEnemy)
        {
            actionPoints = 2;
        }
        else
        {
            actionPoints = 5;
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
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

    public bool DoesHasGun()
    {
        return hasGun;
    }

    public bool DoesHasMelee()
    {
        return hasMelee;
    }

    public void SetGun(bool hasOrNot)
    {
        hasGun = hasOrNot;
        OnAnyUnitPickUpItems?.Invoke(this, EventArgs.Empty);
    }

    public void SetMelee(bool hasOrNot)
    {
        hasMelee = hasOrNot;
        OnAnyUnitPickUpItems?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
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
            SetActionPoints();

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        GridSystemVisual.Instance.UpdateVisualGrid();
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
        Instantiate(bloodSplashVFXPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
    }
    
}
