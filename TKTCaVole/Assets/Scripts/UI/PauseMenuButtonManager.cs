using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuButtonManager : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private SettingsManager settingsManager;
    
    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeButtonAction);
        mainMenuButton.onClick.AddListener(MainMenuButtonAction);
        settingsButton.onClick.AddListener(OpenSettings);
        restartButton.onClick.AddListener(RestartButtonAction);
    }

    private void MainMenuButtonAction()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void ResumeButtonAction()
    { 
        GameInputManager.InvokePause(new InputAction.CallbackContext());
    }
    
    private void RestartButtonAction()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenSettings()
    {
        gameObject.SetActive(false);
        
        settingsManager.Open(settingsButton,ReopenPauseMenu);

        void ReopenPauseMenu()
        {
            gameObject.SetActive(true);
        }
    }
}
