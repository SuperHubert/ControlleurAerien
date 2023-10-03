using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isRight;
    private float step = 0.025f;
    
    public void PingPong()
    {
        if (isRight) transform.position += transform.forward * step;
        else transform.position += transform.forward * -step;
        if (isRight && transform.position.z > 4.0f) isRight = false;
        if(!isRight && transform.position.z < 0.0f) isRight = true;
    }


    private void Update()
    {
        PingPong();
    }
}
