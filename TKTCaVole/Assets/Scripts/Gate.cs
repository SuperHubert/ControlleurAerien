using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public static int GatesLeft { get; private set; }
    public static event Action<int> OnGatesLeftUpdated; 
    
    public static void InitGates(int totalGates)
    {
        GatesLeft = totalGates;
        OnGatesLeftUpdated = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO - Better Destroy;
        Destroy(gameObject);
        
        GatesLeft--;
        
        Debug.Log($"{GatesLeft} gates left");
        
        OnGatesLeftUpdated?.Invoke(GatesLeft);
    }
}
