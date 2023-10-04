using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuButtonManager : MonoBehaviour
{
    [SerializeField] private Button resumeButton, mainMenuButton;
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeButtonAction);
        mainMenuButton.onClick.AddListener(MainMenuButtonAction);
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
}
