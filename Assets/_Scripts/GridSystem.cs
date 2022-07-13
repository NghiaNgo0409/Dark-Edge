using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    float width;
    float height;
    int cellSize;

    public GridSystem(float width, float height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .5f, Color.cyan, float.MaxValue);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize;
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
                GameObject.Instantiate(gridObject, GetWorldPosition(x, z), Quaternion.identity);
            }
        }
    }
}
