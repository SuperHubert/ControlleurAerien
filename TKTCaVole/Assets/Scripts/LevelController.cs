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

    [Header("Debug")]
    [SerializeField] private bool running;
    [SerializeField] private float timer;
    [SerializeField] private float totalTime;

    [Header("Ship")]
    [SerializeField] private GameObject shipGo;
    [SerializeField] private string  addTimerKeyAudio= "TimerIncrease";
    [SerializeField] private string audioGameOver = "GameOver";

    public static event Action<bool,float> OnLevelEnd;
    public static event Action<float> OnTimerUpdated;
    public static event Action<float> OnTotalTimerUpdated;
    public static event Action<float> OnTimerAdded; 

    
    private void Start()
    {
        running = false;
        shipGo.SetActive(false);
        
        var level = LevelTracker.CurrentLevel;
        
        generator.GenerateLevel(level,OnLevelGenerated);
    }

    private void OnLevelGenerated()
    {
        shipGo.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

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
        
        AudioManager.Instance.PlaySound(addTimerKeyAudio);
        
        OnTimerAdded?.Invoke(_timerIncrease);
        
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
        Cursor.visible = true;

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
        AudioManager.Instance.PlaySound(audioGameOver);
        
        LoseLevel();
    }

    public static string ScoreToText(float score)
    {
        var minutes = score / 60;
        minutes -= minutes % 1;
        var seconds = score % 60;
        var deci = score % 1;

        seconds -= deci;
        deci *= 100;

        return $"{minutes:00}:{seconds:00}:{deci:00}";
    }
}
