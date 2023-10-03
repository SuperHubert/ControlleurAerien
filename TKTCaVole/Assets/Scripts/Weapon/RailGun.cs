using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : BulletParent
{
    [SerializeField] private TrailRenderer renderer;
    protected override void OnEnable()
    {
        base.OnEnable();
       
        //faire le raycast ici
        Physics.Raycast(transform.position, transform.forward, 1000.0f);
        renderer.enabled = true;
        //tu tp le railgun a la position du hit
        
    }

    private void OnDisable()
    {
        renderer.enabled = false;
    }
}
