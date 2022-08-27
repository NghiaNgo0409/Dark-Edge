using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Unit unit;
    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            unit.GetMoveAction().GetValidActionGridPosition();
        }
    }
}
