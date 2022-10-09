using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip akSFX, zombieEatingSFX, meleeSFX;
    private void OnEnable()
    {
        ShootAction.OnAnyShootActions += ShootAction_OnAnyShootActions;
    }

    private void OnDisable()
    {
        ShootAction.OnAnyShootActions -= ShootAction_OnAnyShootActions;
    }
    // Start is called before the first frame update
    void Start()
    {
        //ShootAction.OnAnyShootActions += ShootAction_OnAnyShootActions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaySFX(AudioClip sfx)
    {
        sfxSource.clip = sfx;
        sfxSource.Play();
    }

    void ShootAction_OnAnyShootActions(object sender, EventArgs e) 
    {
        PlaySFX(akSFX);
    }
}
