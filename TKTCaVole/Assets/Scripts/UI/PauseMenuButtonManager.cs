using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuButtonManager : MonoBehaviour
{
    [SerializeField] private Button resumeButton, mainMenuButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private SettingsManager settingsManager;
    
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeButtonAction);
        mainMenuButton.onClick.AddListener(MainMenuButtonAction);
        settingsButton.onClick.AddListener(OpenSettings);
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

    private void OpenSettings()
    {
        gameObject.SetActive(false);
        
        settingsManager.Open(resumeButton,ReopenPauseMenu);

        void ReopenPauseMenu()
        {
            gameObject.SetActive(true);
        }
    }
}
