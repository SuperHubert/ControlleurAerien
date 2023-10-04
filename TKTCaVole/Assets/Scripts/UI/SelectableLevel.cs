using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectableLevel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    private GameObject HighScoreGo => highScoreText.gameObject;
    [SerializeField] private RectTransform levelTr;

    [Header("Settings")]
    [SerializeField] private float randomHeightRange = 1080f/2f;
    [SerializeField] private float maxRandomHeight = 1080f/2f;
    [Header("Data")]
    [SerializeField] private int levelId;

    private Selectable downSelectable;

    private void Start()
    {
        button.onClick.AddListener(LaunchLevel);
    }

    public void Select()
    {
        button.Select();
    }

    public void OnButtonSelected()
    {
        var otherNav = downSelectable.navigation;
        otherNav.selectOnUp = button;
        downSelectable.navigation = otherNav;
    }

    public void InitButton(int id,Selectable selectable)
    {
        levelId = id; //0 is level 1
        
        levelText.text = $"Level {levelId+1}";

        gameObject.name = $"Selectable Level {levelId+1}";
        
        UpdateSelectable(selectable);
        
        UpdateLevelHighscore();
        
        UpdateHeight();
    }

    private void UpdateSelectable(Selectable selectable)
    {
        downSelectable = selectable;
        
        var selfNav = button.navigation;
        selfNav.selectOnDown = downSelectable;
        button.navigation = selfNav;
    }

    private void UpdateHeight()
    {
        if(levelId == 0) return;
        
        Random.InitState(0);

        var pos = levelTr.anchoredPosition;

        var x = (levelId % 100) + 3.14f;
        var f3 = 3 * Mathf.Sin(x) / x;
        var y = 2 * Mathf.PerlinNoise1D(3 * x) + f3;

        pos.y = y * randomHeightRange;
        
        levelTr.anchoredPosition = pos;
    }

    private void UpdateLevelHighscore()
    {
        var highscore = LevelTracker.GetLevelHighscore(levelId);
        
        HighScoreGo.SetActive(highscore >= 0);
        
        highScoreText.text = $"{LevelController.ScoreToText(highscore)}";
    }

    private void LaunchLevel()
    {
        LevelTracker.LaunchLevel(levelId);
    }
}
