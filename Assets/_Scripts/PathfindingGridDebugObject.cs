using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using  UnityEditor;
#endif

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] TextMeshPro gCostText;
    [SerializeField] TextMeshPro hCostText;
    [SerializeField] TextMeshPro fCostText;
    PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        // base.Update();
        // gCostText.text = pathNode.GetGCost().ToString();
        // hCostText.text = pathNode.GetHCost().ToString();
        // fCostText.text = pathNode.GetFCost().ToString();

    }

    private void OnDrawGizmosSelected(){
#if UNITY_EDITOR
        Handles.Label(transform.position, pathNode.ToString());
#endif
    }
}
