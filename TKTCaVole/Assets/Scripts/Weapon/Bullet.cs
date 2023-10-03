using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : BulletParent
{
    void OnEnable()
    {
        StartCoroutine(FinalCountDown());
    }

    private void Update()
    {
        rg.velocity = transform.forward * speed;
    }
}
