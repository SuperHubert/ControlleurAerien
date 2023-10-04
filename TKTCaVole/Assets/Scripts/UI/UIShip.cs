using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIShip : MonoBehaviour
{
    [SerializeField] private ShipController controller;
    
    [SerializeField] private TextMeshProUGUI gearText;
    [SerializeField] private TextMeshProUGUI gateLeftText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Camera cam;

    [SerializeField] private Selectable pauseMenuSelectable;
    [SerializeField] private Selectable endGamePanelWinSelectable;
    [SerializeField] private Selectable endGamePanelLoseSelectable;
    
    private float animDuration = 1f;
    [Header("Anim FOV")]
    [SerializeField]
    private AnimationCurve curve;
    private Coroutine fovRoutine;
    

    
    
    private bool isGamePaused = false;
    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;
    [Header("Pause")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI wonLossText, highScoreText;
    private void Start()
    {
        //gateLeftText.text = $"{Gate.TotalGates}/{Gate.TotalGates}";

        controller.OnGearChanged += UpdateGearText;
        GameInputManager.OnPausePerformed += OnPausePerformed;
        Gate.OnGatesLeftUpdated += OnGateLeftUpdate;
        LevelController.OnTimerUpdated += OnTimerUpdated;
        LevelController.OnLevelEnd += OnLevelEnd;
    }

    private void OnLevelEnd(bool won, float score)
    {
        controller.OnGearChanged -= UpdateGearText;
        GameInputManager.OnPausePerformed -= OnPausePerformed;
        Gate.OnGatesLeftUpdated -= OnGateLeftUpdate;
        LevelController.OnTimerUpdated -= OnTimerUpdated;
        LevelController.OnLevelEnd -= OnLevelEnd;
        
        Time.timeScale = 0f;
        endGamePanel.SetActive(true);

        var selectable = won ? endGamePanelWinSelectable : endGamePanelLoseSelectable;
        selectable.Select();
        
        Cursor.lockState = CursorLockMode.None;
        wonLossText.text = won ? "You Won !" : "You Lost !";

        var scoreText = score > 0 ? $"Your score is {score}\n" : "";
        
        highScoreText.text = $"{scoreText}Your HighScore is {LevelTracker.GetLevelHighscore(LevelTracker.CurrentLevel)}";
    }

    private void OnTimerUpdated(float timer)
    {
        timerText.text = $"{LevelController.ScoreToText(timer)}s";
    }

    private void OnGateLeftUpdate(int gateLeft, int gateTotal)
    {
        gateLeftText.text = $"{gateLeft}/{gateTotal}";
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        isGamePaused = !isGamePaused; 
        pausePanel.SetActive(isGamePaused);
        
        if(isGamePaused) pauseMenuSelectable.Select();
        
        Time.timeScale = isGamePaused?0:1;
    }

    private void UpdateGearText(int gear)
    {
        gearText.text = $"{gear}";

        cam.DOKill();
        cam.DOFieldOfView(40 + gear * 5, animDuration).SetEase(curve);
        
        /*
        if(fovRoutine != null) StopCoroutine(fovRoutine);
        fovRoutine = StartCoroutine(LerpFOV(gear, animDuration)); //changer le FOV
        */
    }
    
    
}
