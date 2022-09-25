using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance {get; private set;}

    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;
    [SerializeField] Transform gridPrefab;
    [SerializeField] LayerMask obstacleLayer;
    int width;
    int height;
    float cellSize;
    GridSystem<PathNode> gridSystem;
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
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => 
        {
            var pathNode = new PathNode();
            pathNode.InitPathNode(gridPosition);
            return pathNode;
        });
        gridSystem.CreateGridObject(gridPrefab);

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffset = 5f;
                RaycastHit hit;
                if(Physics.Raycast(worldPosition + Vector3.down * raycastOffset, Vector3.up * raycastOffset, out hit, raycastOffset, obstacleLayer))
                {
                    // GetNode(x, z).HitInfo = hit;
                    GetNode(x, z).SetHitInfo(hit);
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }


    List<PathNode> openList = new List<PathNode>(1000);
    List<PathNode> closedList = new List<PathNode>(1000);
    PathNode startNode;
    PathNode endNode;
    PathNode pathNode;
    PathNode currentNode;
    // Tuple<List<GridPosition>, int> findPathData;

    public struct FindPathData
    {
        public List<GridPosition> GridPositions;
        public int PathLength;
    }

    // public async Task<Tuple<List<GridPosition>, int>>
    public async Task<FindPathData?> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, int pathLength)
    {
        // List<PathNode> openList = new List<PathNode>();
        // List<PathNode> closedList = new List<PathNode>();

        openList.Clear();
        closedList.Clear();

        startNode = gridSystem.GetGridObject(startGridPosition);
        endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                pathNode = GetNode(x, z);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                // pathNode.ResetCameFromPathNode();
                pathNode.ResetCameFromGrid();

                gridSystem.gridObjectArray[x, z] = pathNode;
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        // Update start node in open list.
        for (int i = 0; i < openList.Count; i++)
        {
            PathNode node = openList[i];
            if (node.GetGridPosition() == startNode.GetGridPosition())
            {
                node.SetGCost(startNode.GetGCost());
                node.SetHCost(startNode.GetHCost());
                node.CalculateFCost();

                openList[i] = node;
                break;
            }
        }

        while (openList.Count > 0)
        {
            currentNode = GetLowestFCostPathNode(openList);

            if (currentNode.GetGridPosition() == endNode.GetGridPosition())
            {
                //Reach final node
                pathLength = endNode.GetFCost();
                // var completed = Task.WhenAll(CalculatePath(endNode)).IsCompleted;
                var gridPositions = await CalculatePath(endNode);
                FindPathData findPathData = new FindPathData {
                    GridPositions = gridPositions,
                    PathLength = pathLength
                };
                return findPathData;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if(closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if(!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    // neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetCameFromGrid(currentNode.GetGridPosition());
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //No path found
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance =  Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return Mathf.Min(xDistance, zDistance) * MOVE_DIAGONAL_COST + remaining * MOVE_STRAIGHT_COST;
    }

    PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    public PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    List<PathNode> neighbourNodeList = new List<PathNode>();
    List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        // neighbourNodeList = new List<PathNode>();
        neighbourNodeList.Clear();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourNodeList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up
                neighbourNodeList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
            if (gridPosition.z - 1 >= 0)
            {
                //Left Down
                neighbourNodeList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighbourNodeList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up
                neighbourNodeList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
            if (gridPosition.z - 1 >= 0)
            {
                //Right Down
                neighbourNodeList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourNodeList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }
        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourNodeList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }
        return neighbourNodeList;
    }

    List<GridPosition> gridPositionList = new List<GridPosition>();
    async Task <List<GridPosition>> CalculatePath(PathNode endNode)
    {
        // List<PathNode> pathNodeList = new List<PathNode>();
        gridPositionList.Clear();
        gridPositionList.Add(endNode.GetGridPosition());

        //
        // PathNode currentNode = endNode;
        // while(currentNode.GetCameFromPathNode() != null)
        // {
        //     pathNodeList.Add(currentNode.GetCameFromPathNode());
        //     currentNode = currentNode.GetCameFromPathNode();
        // }
        // pathNodeList.Reverse();
        // // List<GridPosition> gridPositionList = new List<GridPosition>();
        // gridPositionList.Clear();
        // foreach(PathNode pathNode in pathNodeList)
        // {
        //     gridPositionList.Add(pathNode.GetGridPosition());
        // }

        //
         PathNode currentNode = endNode;
        while(currentNode.GetCameFromGrid() != null)
        {
            gridPositionList.Add(currentNode.GetCameFromGrid().Value);
            currentNode = gridSystem.GetGridObject(currentNode.GetCameFromGrid().Value);
        }
        gridPositionList.Reverse();
        // List<GridPosition> gridPositionList = new List<GridPosition>();
        gridPositionList.Clear();
        return await Task.FromResult(gridPositionList);
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        int pathLength = 0;
        var findPathTask = Task.WhenAny(FindPath(startGridPosition, endGridPosition, pathLength));
        // FindPath(startGridPosition, endGridPosition, out int pathLength);
        return findPathTask.Result.Result != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        int pathLength = 0;
        var findPathTask = Task.WhenAny(FindPath(startGridPosition, endGridPosition, pathLength));
        // FindPath(startGridPosition, endGridPosition, out int pathLength);
        return findPathTask.Result.Result.Value.PathLength;
    }
}
