using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GridSystem gridSystem;
    [SerializeField] Transform gridPrefab;
    void Start()
    {
        gridSystem = new GridSystem(10, 10, 2);
        // Debug.Log(new GridPosition(5, 7));
        gridSystem.CreateGridObject(gridPrefab);
    }

    void Update()
    {
        Debug.Log(gridSystem.GetGridPosition(MousePosition.GetPosition()));
    }
}
