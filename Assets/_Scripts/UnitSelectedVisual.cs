using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] Unit unit;
    MeshRenderer meshRenderer;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += Event_SelectingUnitVisualChange;

        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Event_SelectingUnitVisualChange(object sender, EventArgs args)
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if(UnitActionSystem.Instance.GetSelectingUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
