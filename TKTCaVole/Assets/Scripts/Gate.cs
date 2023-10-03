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
        //TODO - Better Destroy;
        Destroy(gameObject);
        
        GatesLeft--;
        
        Debug.Log($"{GatesLeft}/{TotalGates} gates left");
        
        OnGatesLeftUpdated?.Invoke(GatesLeft,TotalGates);

        if (GatesLeft > 0) return;
        
        CompleteLevel();
    }

    [ContextMenu("Complete Level")]
    private void CompleteLevel()
    {
        //TODO - Transition, score math
        
        LevelTracker.CompleteLevel(69);
    }
}
