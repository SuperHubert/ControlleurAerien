using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private LevelGenerator generator;

    [Header("Settings")]
    [SerializeField] private float timerStart = 30f;
    [SerializeField] private float timerDecreasePerLevel = 2.5f;
    [SerializeField] private float minTime = 15f;
    [SerializeField] private float decayRate = 1f;
    [SerializeField] private float timerIncrease = 15f;

    [Header("Debug")]
    [SerializeField] private bool running;
    [SerializeField] private float timer;
    [SerializeField] private float totalTime;

    public static event Action<bool,float> OnLevelEnd;
    public static event Action<float> OnTimerUpdated;
    public static event Action<float> OnTotalTimerUpdated;

    
    private void Start()
    {
        running = false;

        var level = LevelTracker.CurrentLevel;
        
        generator.GenerateLevel(level,OnLevelGenerated);
    }

    private void OnLevelGenerated()
    {
        //TODO - setup stuff here

        Gate.OnGatesLeftUpdatedTimer += IncreaseTimer;
        Gate.OnGatesLeftUpdated += TryWinLevel;
        Hourglass.OnHourglassCollected += IncreaseTimer;
        Plane.OnPlaneDestroyed += LoseLevel;

        var level = LevelTracker.CurrentLevel;
        
        timer = timerStart - level * timerDecreasePerLevel;
        if (timer < minTime) timer = minTime;
        
        running = true;
    }

    private void IncreaseTimer(float _timerIncrease)
    {
        timer += _timerIncrease;
        
        //TODO mettre un genre +00:15 au dessus de l'ui du timer
        OnTimerUpdated?.Invoke(timer);
    }

    private void TryWinLevel(int gatesLeft,int _)
    {
        if(gatesLeft > 0) return;
        
        WinLevel();
    }

    private void EndLevel()
    {
        running = false;
        
        Gate.OnGatesLeftUpdatedTimer -= IncreaseTimer;
        Gate.OnGatesLeftUpdated -= TryWinLevel;
        Hourglass.OnHourglassCollected -= IncreaseTimer;
    }

    [ContextMenu("Force Win")]
    private void WinLevel()
    {
        EndLevel();
        
        //TODO - Transition, score math

        var score = totalTime;
        
        OnLevelEnd?.Invoke(true,score);
        
        LevelTracker.CompleteLevel(score);
    }

    [ContextMenu("Force Lose")]
    private void LoseLevel()
    {
        EndLevel();
        
        // TODO - Display lost UI, (send event)
        
        OnLevelEnd?.Invoke(false,-1);
    }

    private void Update()
    {
        if(!running) return;

        timer -= decayRate * Time.deltaTime;
        totalTime += decayRate * Time.deltaTime;
        
        OnTimerUpdated?.Invoke(timer);
        OnTotalTimerUpdated?.Invoke(totalTime);
        
        if(timer > 0) return;
        
        LoseLevel();
    }

    public static string ScoreToText(float score)
    {
        var minutes = score / 60;
        var seconds = score % 60;
        var deci = score % 1;

        seconds -= deci;
        deci *= 100;

        return $"{minutes:00}:{seconds:00}:{deci:00}";
    }
}
