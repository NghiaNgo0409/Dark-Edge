using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    int width;
    int height;
    int cellSize;

    GridObject[,] gridObjectArray;

    public GridSystem(int width, int height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return ( new GridPosition(
            Mathf.RoundToInt(worldPosition.x) / cellSize,
            Mathf.RoundToInt(worldPosition.z) / cellSize
        ));
    }

    public void CreateGridObject(Transform gridObject)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugObject = GameObject.Instantiate(gridObject, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObject.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
                gridPosition.z >= 0 && 
                gridPosition.x < width && 
                gridPosition.z < height;
    }
    
    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
