using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelEndGameManager : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton, restartButton;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenuButtonAction);
        restartButton.onClick.AddListener(RestartButtonAction);
    }

    private void RestartButtonAction()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MainMenuButtonAction()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
