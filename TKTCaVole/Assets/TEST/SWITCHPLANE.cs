using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SWITCHPLANE : MonoBehaviour
{
    [SerializeField] private GameObject H;

    private void Start()
    {
        StartCoroutine(P());
    }

    private IEnumerator P()
    {
        Instantiate(H, new Vector3(0, 0, 80), Quaternion.identity);
        yield return new WaitForSeconds(3);
        StartCoroutine(P());
    }
}