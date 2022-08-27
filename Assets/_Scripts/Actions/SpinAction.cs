using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    float totalSpinAmount;
    public Action onSpinCompleted;
    protected override void Awake() 
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isActive)
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            totalSpinAmount += spinAddAmount;
        }

        if(totalSpinAmount > 360f)
        {
            isActive = false;
            onSpinCompleted();
        }
    }

    public void Spin(Action onSpinCompleted)
    {
        this.onSpinCompleted = onSpinCompleted;
        totalSpinAmount = 0;
        isActive = true;
    }
}
