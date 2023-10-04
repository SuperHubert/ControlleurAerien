using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    
    IEnumerator FinalCountDown()
    {
        yield return new WaitForSeconds(lifeTime);
        DebrisPoolManager.instance.AddDebrisToPool(gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(FinalCountDown());
    }
}
