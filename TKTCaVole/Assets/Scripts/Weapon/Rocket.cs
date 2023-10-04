using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : BulletParent
{
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        rg.velocity = transform.forward * speed;
    }
}
