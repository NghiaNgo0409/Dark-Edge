using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShootAction.OnAnyShootActions += ShootAction_OnAnyShootActions;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShootAction_OnAnyShootActions(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake();
    }

    void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }
}
