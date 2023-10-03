using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : BulletParent
{
    private void Update()
    {
        rg.velocity = transform.forward * speed;
    }
}
