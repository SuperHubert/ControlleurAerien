using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
    public static event Action<float> OnHourglassCollected;

    [SerializeField] private float timeAdded = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float startDissolveKey = 10;
    [SerializeField] private float endDissolveKey = -10;
    [SerializeField] private float speedDissolve = 120;
    [SerializeField] private List<Renderer> rends;
    [SerializeField] private Material referenceMat;

    private static readonly int CutoffHeight = Shader.PropertyToID("_CutoffHeight");


    private void Start()
    {
        rb.angularVelocity = new Vector3(0, 0, 5f);
        foreach (var rend in rends)
        {
            var mat = new Material(referenceMat);
            rend.material = null;

            mat.SetFloat(CutoffHeight, startDissolveKey);
            rend.material = mat;
        }
    }

    private IEnumerator DissolveManagement()
    {
        bool Dissolve = false;

        while (!Dissolve)
        {
            foreach (var render in rends)
            {
                render.material.SetFloat(CutoffHeight,
                    render.material.GetFloat(CutoffHeight) - Time.deltaTime * speedDissolve);
                if (render.material.GetFloat(CutoffHeight) <= endDissolveKey)
                    Dissolve = true;
            }

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHourglassCollected?.Invoke(timeAdded);
        StartCoroutine(DissolveManagement());
    }
}