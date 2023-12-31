using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gate : MonoBehaviour
{
    public static int GatesLeft { get; private set; }
    public static int TotalGates { get; private set; }
    public static event Action<int, int> OnGatesLeftUpdated;
    public static event Action<float> OnGatesLeftUpdatedTimer;

    [SerializeField] private float increaseTimer = 15f;
    [SerializeField] private Animator animator;
    [Header("Dissolve")]
    [SerializeField] private List<Renderer> matDissolver;
    [SerializeField] private float speedDissolve = 240;
    [SerializeField] private float limitDissolve = -500.0f;
    [SerializeField] private string GateFinishKeySound = "GateFinish";

    [FormerlySerializedAs("collider")] [SerializeField]
    private Collider col;
    
    private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");
    private static readonly int CutoffHeight = Shader.PropertyToID("_CutoffHeight");

    public static void InitGates(int totalGates)
    {
        TotalGates = totalGates;
        GatesLeft = TotalGates;

        OnGatesLeftUpdated?.Invoke(GatesLeft, TotalGates);
    }

    private void OnEnable()
    {
        foreach (var rend in matDissolver)
        {
            var mat = new Material(rend.material);
            
            mat.SetFloat(CutoffHeight, 130);
            rend.material = mat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerGate();
    }

    private IEnumerator DissolveManagement()
    {
        bool Dissolve = false;

        while (!Dissolve)
        {
            foreach (var render in matDissolver)
            {
                render.material.SetFloat(CutoffHeight,
                    render.material.GetFloat(CutoffHeight) - Time.deltaTime * speedDissolve);
                if (render.material.GetFloat(CutoffHeight) <= limitDissolve)
                    Dissolve = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    [ContextMenu("Trigger")]
    private void TriggerGate()
    {
        //TODO - Better Destroy;
        AudioManager.Instance.PlaySound(GateFinishKeySound);
        col.enabled = false;
        animator.SetTrigger(DestroyTrigger);
        StartCoroutine(DissolveManagement());

        GatesLeft--;

        OnGatesLeftUpdated?.Invoke(GatesLeft, TotalGates);
        OnGatesLeftUpdatedTimer?.Invoke(increaseTimer);
    }
}