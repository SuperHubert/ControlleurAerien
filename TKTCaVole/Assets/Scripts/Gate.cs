using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public static int GatesLeft { get; private set; }
    public static int TotalGates { get; private set; }
    public static event Action<int,int> OnGatesLeftUpdated; 
    
    public static void InitGates(int totalGates)
    {
        TotalGates = totalGates;
        GatesLeft = TotalGates;
        OnGatesLeftUpdated = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerGate();
    }

    [ContextMenu("Trigger")]
    private void TriggerGate()
    {
        //TODO - Better Destroy;
        Destroy(gameObject);
        
        GatesLeft--;
        
        Debug.Log($"{GatesLeft}/{TotalGates} gates left");
        
        OnGatesLeftUpdated?.Invoke(GatesLeft,TotalGates);
    }
}
