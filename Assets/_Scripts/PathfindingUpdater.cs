using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DestructibleCrate.OnAnyCrateDestroyed += DestructibleCrate_OnAnyCrateDestroyed; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestructibleCrate_OnAnyCrateDestroyed(object sender, EventArgs e) 
    {
        DestructibleCrate crate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(crate.GetGridPosition(), true);
    }
}
