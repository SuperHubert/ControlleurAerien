using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIShip : MonoBehaviour
{
    [SerializeField] private ShipController controller;
    
    [SerializeField] private TextMeshProUGUI gateLeftText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Camera cam;

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
    [Header("End Game")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI wonLossText, highScoreText;
    
    private void Start()
    {
        pausePanel.SetActive(false);
        endGamePanel.SetActive(false);

        gearFeedbackImage.fillAmount = 1/6f;

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
        wonLossText.text = won ? "you won !" : "you lost !";

        var scoreText = score > 0 ? $"time : {LevelController.ScoreToText(score)}\n" : "";
        var highscore = LevelTracker.GetLevelHighscore(LevelTracker.CurrentLevel);
        var highscoreText = highscore > 0 ? $"record : {LevelController.ScoreToText(highscore)}" : "";
        
        highScoreText.text = $"{scoreText}{highscoreText}";
    }

    private void OnTimerUpdated(float timer)
    {
        timerText.text = $"{LevelController.ScoreToText(timer)}s";
    }

    private void OnGateLeftUpdate(int gateLeft, int gateTotal)
    {
        gateLeftText.text = $"pass gates {gateTotal-gateLeft}/{gateTotal}";
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
        gearFeedbackImage.DOFillAmount((gear + 1) / 6f, animDuration).SetEase(curve);
        
        cam.DOKill();
        cam.DOFieldOfView(40 + gear * 5, animDuration).SetEase(curve);
        
        /*
        if(fovRoutine != null) StopCoroutine(fovRoutine);
        fovRoutine = StartCoroutine(LerpFOV(gear, animDuration)); //changer le FOV
        */
    }
    
    
}
