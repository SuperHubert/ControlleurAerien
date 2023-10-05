using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float speedDissolve = 240;
    [SerializeField] private float limitDissolve = -300.0f;
    
    private IEnumerator DissolveManagement()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        while (renderer.material.GetFloat("_CutoffHeight") > limitDissolve)
        {
            renderer.material.SetFloat("_CutoffHeight",
                renderer.material.GetFloat("_CutoffHeight") - Time.deltaTime * speedDissolve);
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
