using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float speedDissolve = 240;
    [SerializeField] private float limitDissolve = -300.0f;
    [SerializeField] private Renderer rend;

    private void Start()
    {
        rend.material = new Material(rend.material);
    }

    private IEnumerator DissolveManagement()
    {
        while (GetComponent<Renderer>().material.GetFloat("_CutoffHeight") > limitDissolve)
        {
            GetComponent<Renderer>().material.SetFloat("_CutoffHeight",
                GetComponent<Renderer>().material.GetFloat("_CutoffHeight") - Time.deltaTime * speedDissolve);
            yield return new WaitForEndOfFrame();
        }
        DebrisPoolManager.instance.AddDebrisToPool(gameObject);
    }

    
    IEnumerator FinalCountDown()
    {
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(DissolveManagement());
        //DebrisPoolManager.instance.AddDebrisToPool(gameObject);
        
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_CutoffHeight", 50);
        StartCoroutine(FinalCountDown());
    }
}
