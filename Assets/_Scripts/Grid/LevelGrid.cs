using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    GridSystem gridSystem;
    [SerializeField] Transform gridPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateGridObject(gridPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
