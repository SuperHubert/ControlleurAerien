using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private int number;

    public void SetNumber(int value)
    {
        number = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }
}
