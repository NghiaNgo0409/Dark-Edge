using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    [SerializeField] Unit selectingUnit;
    [SerializeField] LayerMask unitLayerMask;
    BaseAction selectedAction;
    bool isBusy;

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
        SetSelectingUnit(selectingUnit);
    }

    // Update is called once per frame
    void Update()
    {
        if(isBusy) return;
        if(TryHandleSelectingUnit()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;

        HandleSelectionAction();
    }

    void HandleSelectionAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePosition.GetPosition());

            if(selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
    }

    bool TryHandleSelectingUnit()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitRay, float.MaxValue, unitLayerMask))
            {
                if(hitRay.collider.TryGetComponent<Unit>(out Unit unit))
                {
                    if(selectingUnit == unit)
                    {
                        return false;
                    }
                    SetSelectingUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    public void SetSelectingUnit(Unit unit)
    {
        selectingUnit = unit;
        SetSelectingAction(unit.GetMoveAction());
        
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty); //Fire off the event. In this case is a selecting unit event
    }

    public void SetSelectingAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectingUnit()
    {
        return selectingUnit;
    }

    public BaseAction GetSelectimgAction()
    {
        return selectedAction;
    }

    public void SetBusy()
    {
        isBusy = true;
    }

    public void ClearBusy()
    {
        isBusy = false;
    }
}
