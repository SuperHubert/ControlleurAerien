using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DebrisPoolManager : MonoBehaviour
{
    public static DebrisPoolManager instance;
    [SerializeField] private GameObject[] debrisPrefabs;
    [SerializeField] private GameObject HourglassPrefab;
    [SerializeField] private int poolSizeDebris;
    [SerializeField] private int poolSizeHourglass;

    private List<GameObject> debrisPool = new ();
    private List<GameObject> hourglassPool = new ();

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < debrisPrefabs.Length; i++)
        {
            for (int j = 0; j < poolSizeDebris; j++)
            {
                GameObject obj = Instantiate(debrisPrefabs[i], transform);
                obj.SetActive(false);
                debrisPool.Add(obj);
            }
        }

        for (int i = 0; i < poolSizeHourglass; i++)
        {
            GameObject obj = Instantiate(HourglassPrefab, transform);
            obj.SetActive(false);
            debrisPool.Add(obj);
        }
    }

    public GameObject GetRandomDebris()
    {
        GameObject obj = null;
        if (debrisPool.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, debrisPool.Count);
            obj = debrisPool[index];
            debrisPool.RemoveAt(index);
        }
        else
        {
            obj = Instantiate(debrisPrefabs[Random.Range(0, debrisPrefabs.Length)], transform);
        }
        obj.SetActive(true);
        return obj;
    }
    
    public void AddDebrisToPool(GameObject debris)
    {
        debris.SetActive(false);
        debrisPool.Add(debris);
    }
    
    public GameObject GetHourglass()
    {
        GameObject obj = null;
        if (hourglassPool.Count > 0)
        {
            obj = hourglassPool[0];
            hourglassPool.RemoveAt(0);
        }
        else
        {
            obj = Instantiate(HourglassPrefab, transform);
        }
        obj.SetActive(true);
        return obj;
    }
    
    public void AddHourglassToPool(GameObject hourglass)
    {
        hourglass.SetActive(false);
        hourglassPool.Add(hourglass);
    }
}