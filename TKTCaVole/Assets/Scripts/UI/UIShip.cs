using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIShip : MonoBehaviour
{
    [SerializeField] private ShipController controller;
    
    [SerializeField] private TextMeshProUGUI gateLeftText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Camera cam;

    [Header("Time Warning")]
    [SerializeField] private float timeWarning = 15f;
    [SerializeField] private float blinkFrequency = 0.75f;
    private bool isBlinking;
    private Sequence blinkSequence;
    
    [Header("Extra Time")]
    [SerializeField] private UIExtraTime uiExtraTimePrefab;
    [SerializeField] private Transform uiExtraTimeParent;
    [SerializeField] private int uiExtraTimePoolSize;
    private Queue<UIExtraTime> uiExtraTimeQueue = new ();
    
    [Header("Shooting")]
    [SerializeField] private Image rocketCooldownImage;
    [SerializeField] private List<Graphic> elementsToShow = new();
    private Weapon weapon;
    
    [Header("Selectables")]
    [SerializeField] private Selectable pauseMenuSelectable;
    [SerializeField] private Selectable endGamePanelWinSelectable;
    [SerializeField] private Selectable endGamePanelLoseSelectable;

    [SerializeField] private Image gearFeedbackImage;
    
    private float animDuration = 1f;
    [Header("Anim FOV")]
    [SerializeField]
    private AnimationCurve curve;
    private Coroutine fovRoutine;
    
    private bool isGamePaused = false;
    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private float pausePanelTransitionDuration = 0.25f;
    [Header("End Game")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI wonLossText, highScoreText;
    
    private void Start()
    {
        pausePanel.SetActive(false);
        endGamePanel.SetActive(false);

        isBlinking = false;
        weapon = Plane.Rocket;

        weapon.OnReloadStart += OnReloadStart;

        InitTimerSequence();
        InitUIExtraTime();
        InitRocketCooldown();
        gearFeedbackImage.fillAmount = 1/6f;

        Cursor.lockState = CursorLockMode.Locked;
        
        controller.OnGearChanged += UpdateGearText;
        GameInputManager.OnPausePerformed += OnPausePerformed;
        Gate.OnGatesLeftUpdated += OnGateLeftUpdate;
        LevelController.OnTimerAdded += DisplayExtraTime;
        LevelController.OnTimerUpdated += OnTimerUpdated;
        LevelController.OnLevelEnd += OnLevelEnd;
    }

    private void OnDisable()
    {
        RemoveCallbacks();
    }

    private void RemoveCallbacks()
    {
        rocketCooldownImage.DOKill();
        
        weapon.OnReloadStart -= OnReloadStart;
        controller.OnGearChanged -= UpdateGearText;
        GameInputManager.OnPausePerformed -= OnPausePerformed;
        Gate.OnGatesLeftUpdated -= OnGateLeftUpdate;
        LevelController.OnTimerAdded -= DisplayExtraTime;
        LevelController.OnTimerUpdated -= OnTimerUpdated;
        LevelController.OnLevelEnd -= OnLevelEnd;
    }

    private void OnLevelEnd(bool won, float score)
    {
        RemoveCallbacks();
        
        ShowEndGamePanel(true);

        var selectable = won ? endGamePanelWinSelectable : endGamePanelLoseSelectable;
        selectable.Select();
        
        Cursor.lockState = CursorLockMode.None;
        wonLossText.text = won ? "you won !" : "you lost !";

        var scoreText = score > 0 ? $"time : {LevelController.ScoreToText(score)}\n" : "";
        var highscore = LevelTracker.GetLevelHighscore(LevelTracker.CurrentLevel);
        var highscoreText = highscore > 0 ? $"record : {LevelController.ScoreToText(highscore)}" : "";
        
        highScoreText.text = $"{scoreText}{highscoreText}";
    }
    
    private void ShowEndGamePanel(bool value)
    {
        if(value) endGamePanel.SetActive(true);

        var tr = endGamePanel.transform;

        tr.localScale = new Vector3(1, value ? 0 : 1, 1);
        tr.DOScaleY(value ? 1 : 0, pausePanelTransitionDuration).OnComplete(()=>endGamePanel.SetActive(value)).SetUpdate(true);
    }

    private void InitUIExtraTime()
    {
        for (int i = 0; i < uiExtraTimePoolSize; i++)
        {
            var extraTime = Instantiate(uiExtraTimePrefab, uiExtraTimeParent);
            extraTime.Init();
            
            uiExtraTimeQueue.Enqueue(extraTime);
        }
    }

    private void DisplayExtraTime(float time)
    {
        var extraTime = uiExtraTimeQueue.Dequeue();
        extraTime.PlayAnim(time);
        uiExtraTimeQueue.Enqueue(extraTime);
    }

    private void InitRocketCooldown()
    {
        foreach (var element in elementsToShow)
        {
            var col = element.color;
            col.a = 0;
            element.color = col;
        }
        rocketCooldownImage.fillAmount = 0;
    }

    private void OnReloadStart(float duration)
    {
        foreach (var element in elementsToShow)
        {
            element.DOFade(1, 0.1f);
        }
        
        rocketCooldownImage.fillAmount = 0;
        
        rocketCooldownImage.DOFillAmount(1, duration).OnComplete(RevealElements);
        return;

        void RevealElements()
        {
            foreach (var element in elementsToShow)
            {
                element.DOFade(0, 0.1f);
            }
        }
    }

    private void InitTimerSequence()
    {
        blinkSequence = DOTween.Sequence();

        blinkSequence.AppendCallback(() => timerText.color = Color.red);
        blinkSequence.AppendInterval(blinkFrequency);
        blinkSequence.AppendCallback(() => timerText.color = UISettingsSo.CurrentSettings.White);
        blinkSequence.AppendInterval(blinkFrequency);
        blinkSequence.SetLoops(-1);

        blinkSequence.Pause();
    }

    private void OnTimerUpdated(float timer)
    {
        timerText.text = $"{LevelController.ScoreToText(timer)}";

        var warning = timer < timeWarning;
        
        if (warning && !isBlinking)
        {
            isBlinking = true;
            blinkSequence.Play();
            return;
        }

        if (!warning && isBlinking)
        {
            isBlinking = false;
            timerText.color = UISettingsSo.CurrentSettings.White;
            blinkSequence.Pause();
        }
    }

    private void OnGateLeftUpdate(int gateLeft, int gateTotal)
    {
        gateLeftText.text = $"pass gates {gateTotal-gateLeft}/{gateTotal}";
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        isGamePaused = !isGamePaused; 
        
        ShowPauseMenu(isGamePaused);
        

        if (isGamePaused) pauseMenuSelectable.Select();
        
        Cursor.lockState = isGamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isGamePaused;
        //Debug.Log(isGamePaused ? "UnPause : Unlocked Cursor":"Pause :Locked Cursor");
        Time.timeScale = isGamePaused?0:1;
    }

    private void ShowPauseMenu(bool value)
    {
        if(value) pausePanel.SetActive(true);

        var tr = pausePanel.transform;

        tr.localScale = new Vector3(1, value ? 0 : 1, 1);
        tr.DOScaleY(value ? 1 : 0, pausePanelTransitionDuration).OnComplete(()=>pausePanel.SetActive(value)).SetUpdate(true);
    }

    private void UpdateGearText(int gear)
    {
        gearFeedbackImage.DOFillAmount((gear + 1) / 6f, animDuration).SetEase(curve);
        
        cam.DOKill();
        cam.DOFieldOfView(40 + gear * 5, animDuration).SetEase(curve);
        
        /*
        if(fovRoutine != null) StopCoroutine(fovRoutine);
        fovRoutine = StartCoroutine(LerpFOV(gear, animDuration)); //changer le FOV
        */
    }
    
    
}
