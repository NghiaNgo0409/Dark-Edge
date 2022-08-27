using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedUnitChange;
    [SerializeField] Unit selectingUnit;
    [SerializeField] LayerMask unitLayerMask;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(TryHandleSelectingUnit()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePosition.GetPosition());

            if(selectingUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectingUnit.GetMoveAction().Move(mouseGridPosition);
            }
        }
    }

    bool TryHandleSelectingUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitRay, float.MaxValue, unitLayerMask))
        {
            if(hitRay.collider.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectingUnit(unit);
                return true;
            }
        }
        return false;
    }

    public void SetSelectingUnit(Unit unit)
    {
        selectingUnit = unit;
        
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty); //Fire off the event. In this case is a selecting unit event
    }

    public Unit GetSelectingUnit()
    {
        return selectingUnit;
    }
}
