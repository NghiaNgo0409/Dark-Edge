using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionSystemUIPrefab;
    [SerializeField] Transform actionSystemUIContainer;
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += OnEventChanged_UpdateActionUI;
        CreateActionUIButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateActionUIButtons()
    {
        foreach(Transform actionSingle in actionSystemUIContainer)
        {
            Destroy(actionSingle.gameObject);
        }
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectingUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform baseActionTransform = Instantiate(actionSystemUIPrefab, actionSystemUIContainer);
            ActionButtonUI actionButtonUI = baseActionTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    void OnEventChanged_UpdateActionUI(object sender, EventArgs e) 
    {
        CreateActionUIButtons();
    }
}
