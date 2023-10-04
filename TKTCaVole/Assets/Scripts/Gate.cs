using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Gate : MonoBehaviour
{
    public static int GatesLeft { get; private set; }
    public static int TotalGates { get; private set; }
    public static event Action<int,int> OnGatesLeftUpdated; 
    public static event Action<float> OnGatesLeftUpdatedTimer;
    
    [SerializeField] private float increaseTimer = 15f;
    [SerializeField] private Animator animator;
    [FormerlySerializedAs("collider")] [SerializeField] private Collider col;
    private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");

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
        col.enabled = false;
        animator.SetTrigger(DestroyTrigger);
        
        GatesLeft--;
        
        OnGatesLeftUpdated?.Invoke(GatesLeft,TotalGates);
        OnGatesLeftUpdatedTimer?.Invoke(increaseTimer);
    }
}
