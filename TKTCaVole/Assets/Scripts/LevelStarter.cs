using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private LevelGenerator generator;

    [Header("Settings")]
    [SerializeField] private float timerStart = 15f;
    [SerializeField] private float decayRate = 1f;
    [SerializeField] private float timerIncrease = 15f;
    
    [Header("Debug")]
    [SerializeField] private bool running;
    [SerializeField] private float timer;

    
    private void Start()
    {
        running = false;
        generator.GenerateLevel(LevelTracker.CurrentLevel,OnLevelGenerated);
    }

    private void OnLevelGenerated()
    {
        //TODO - setup stuff here

        Gate.OnGatesLeftUpdated += ResetTimer;

        timer = timerStart;
        
        running = true;
    }

    private void ResetTimer(int _,int __)
    {
        timer += timerIncrease;
    }

    private void Update()
    {
        if(!running) return;

        timer -= decayRate * Time.deltaTime;
        
        if(timer > 0) return;

        running = false;
        
        // TODO - Display lost UI, (send event)
        Debug.Log("Lost");
    }
}
