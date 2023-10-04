using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
    public static event Action<float> OnHourglassCollected; 
    
    [SerializeField] private float timeAdded = 5f;
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        rb.angularVelocity = new Vector3(0, 0, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
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
