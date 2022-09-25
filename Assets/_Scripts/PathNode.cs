using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public struct PathNodeData
// {
//     GridPosition gridPosition;
//     int gCost;
//     int hCost;
//     int fCost;
//     bool isWalkable;
// }

public struct PathNode
{
    GridPosition gridPosition;
    int gCost;
    int hCost;
    int fCost;
    bool isWalkable;
    // PathNode cameFromPathNode;

    GridPosition? camFromGrid;

    public RaycastHit? HitInfo {get; set;}

    public void InitPathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
        isWalkable = true;
        camFromGrid = null;
    }

    public void SetHitInfo(RaycastHit info)
    {
        HitInfo = info;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    {
        return gCost;
    }

    public int GetHCost()
    {
        return hCost;
    }

    public int GetFCost()
    {
        return fCost;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    // public void ResetCameFromPathNode()
    // {
    //     cameFromPathNode = null;
    // }

    // public void SetCameFromPathNode(PathNode currentNode)
    // {
    //     cameFromPathNode = currentNode;
    // }

    // public PathNode GetCameFromPathNode()
    // {
    //     return cameFromPathNode;
    // }

    public void ResetCameFromGrid()
    {
        camFromGrid = null;
    }

    public void SetCameFromGrid(GridPosition gridPos)
    {
        camFromGrid = gridPos;
    }

    public GridPosition? GetCameFromGrid()
    {
        return camFromGrid;
    }
}
