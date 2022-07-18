using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] Unit selectingUnit;
    [SerializeField] LayerMask unitLayerMask;
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

            selectingUnit.Move(MousePosition.GetPosition());
        }
    }

    bool TryHandleSelectingUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitRay, float.MaxValue, unitLayerMask))
        {
            if(hitRay.collider.TryGetComponent<Unit>(out Unit unit))
            {
                selectingUnit = unit;
                return true;
            }
        }
        return false;
    }
}
