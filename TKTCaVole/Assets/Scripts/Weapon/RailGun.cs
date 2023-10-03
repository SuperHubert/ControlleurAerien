using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : BulletParent
{
    protected override void OnEnable()
    {
        base.OnEnable();
        //faire le raycast ici
        Physics.Raycast(transform.position, transform.forward, 1000.0f);
    }
}
