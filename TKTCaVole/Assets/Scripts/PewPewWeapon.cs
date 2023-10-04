using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewPewWeapon : Weapon
{
   
    
    
    public override void Shoot()
    {
        base.Shoot();
        if (!canFire) return;
        
    }
}
