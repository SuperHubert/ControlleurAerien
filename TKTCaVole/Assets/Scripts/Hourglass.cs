using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
    public static event Action<int,int> OnHourglassCollected; 
    
    [SerializeField] private float timeAdded = 5f;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hourglass triggered");
        OnHourglassCollected?.Invoke(0,0);
        // add time to timer
        
        if (DebrisPoolManager.instance)
        {
            DebrisPoolManager.instance.AddHourglassToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
