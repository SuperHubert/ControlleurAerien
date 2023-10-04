using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebrisPoolManager : MonoBehaviour
{
    public static DebrisPoolManager instance;
    [SerializeField] private GameObject[] debrisPrefabs;
    [SerializeField] private int poolSize;

    private List<GameObject> debrisPool = new List<GameObject>();

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < debrisPrefabs.Length; i++)
        {
            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(debrisPrefabs[i], transform);
                obj.SetActive(false);
                debrisPool.Add(obj);
            }
        }
    }

    public GameObject GetRandomDebris()
    {
        GameObject obj = null;
        if (debrisPool.Count > 0)
            obj = debrisPool[UnityEngine.Random.Range(0, debrisPool.Count)];
        else
        {
            obj = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], transform);
        }
        obj.SetActive(true);
        return obj;
    }
    
    public void AddToPool(GameObject debris)
    {
        debris.SetActive(false);
        debrisPool.Add(debris);
    }
}