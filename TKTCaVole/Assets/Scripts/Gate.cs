using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public static int GatesLeft { get; private set; }
    public static int TotalGates { get; private set; }
    public static event Action<int,int> OnGatesLeftUpdated; 
    public static event Action<float> OnGatesLeftUpdatedTimer;
    
    [SerializeField] private float increaseTimer = 15f;
    
    public static void InitGates(int totalGates)
    {
        TotalGates = totalGates;
        GatesLeft = TotalGates;
        
        OnGatesLeftUpdated?.Invoke(GatesLeft,TotalGates);
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
        
        OnGatesLeftUpdated?.Invoke(GatesLeft,TotalGates);
        OnGatesLeftUpdatedTimer?.Invoke(increaseTimer);
    }
}
