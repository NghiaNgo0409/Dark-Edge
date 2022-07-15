using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    GridObject gridObject;
    [SerializeField] TextMeshPro text;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    void Update()
    {
        text.text = gridObject.ToString();
    }
}
