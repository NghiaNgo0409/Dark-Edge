using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance {get; private set;}
    List<Unit> unitList;
    List<Unit> friendlyUnitList;
    List<Unit> enemyUnitList;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

    void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Add(unit);
        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }

        if(!unit.IsEnemy())
        {
            friendlyUnitList.Add(unit);
        }
    }

    void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        if(unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }

        if(!unit.IsEnemy())
        {
            friendlyUnitList.Remove(unit);
        }
    }
}
