using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
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
    [SerializeField] private float totalTime;

    
    private void Start()
    {
        running = false;

        var level = LevelTracker.CurrentLevel;
            
        Debug.Log($"Launching level {level}");
        
        generator.GenerateLevel(level,OnLevelGenerated);
    }

    private void OnLevelGenerated()
    {
        //TODO - setup stuff here

        Gate.OnGatesLeftUpdated += ResetTimer;
        Gate.OnGatesLeftUpdated += TryWinLevel;

        timer = timerStart;
        
        running = true;
    }

    private void ResetTimer(int _,int __)
    {
        timer += timerIncrease;
    }

    private void TryWinLevel(int gatesLeft,int _)
    {
        if(gatesLeft > 0) return;
        
        WinLevel();
    }

    private void EndLevel()
    {
        running = false;
        
        Gate.OnGatesLeftUpdated -= ResetTimer;
        Gate.OnGatesLeftUpdated -= TryWinLevel;
    }

    [ContextMenu("Force Win")]
    private void WinLevel()
    {
        EndLevel();
        
        //TODO - Transition, score math

        var score = totalTime;

        Debug.Log($"Rounded Score is {score} ({ScoreToText(score)})");
        
        LevelTracker.CompleteLevel(score);
    }

    [ContextMenu("Force Lose")]
    private void LoseLevel()
    {
        EndLevel();
        
        // TODO - Display lost UI, (send event)
        
        Debug.Log("Lost");
    }

    private void Update()
    {
        if(!running) return;

        timer -= decayRate * Time.deltaTime;
        totalTime += decayRate * Time.deltaTime;
        
        if(timer > 0) return;
        
        LoseLevel();
    }

    public static string ScoreToText(float score)
    {
        var minutes = score / 60;
        var seconds = score % 60;
        var deci = score % 1;

        seconds -= deci;
        deci *= 1000;

        return $"{minutes:00}:{seconds:00}:{deci:000}";
    }
}
