using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Transform gridPrefab;
    int width;
    int height;
    int cellSize;
    GridSystem<PathNode> gridSystem;
    void Awake()
    {
        gridSystem = new GridSystem<PathNode>(10, 10, 2, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateGridObject(gridPrefab);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
