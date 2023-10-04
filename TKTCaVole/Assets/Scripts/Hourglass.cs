using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
    public static event Action<float> OnHourglassCollected; 
    
    [SerializeField] private float timeAdded = 5f;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hourglass triggered");
        OnHourglassCollected?.Invoke(timeAdded);
        
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
