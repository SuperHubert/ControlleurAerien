using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : BulletParent
{
    private Transform target;
    
    public void AssignTarget(Transform _target)
    {
        target = _target;
        StartCoroutine(FinalCountDown());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        rg.velocity = transform.forward * speed;
    }
}
