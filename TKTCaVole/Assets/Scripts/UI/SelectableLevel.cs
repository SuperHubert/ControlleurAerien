using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectableLevel : MonoBehaviour
{
    [field:Header("Components")]
    [field:SerializeField] public Button Button { get; private set; }
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
        Button.onClick.AddListener(LaunchLevel);
    }

    public void Select()
    {
        Button.Select();
    }

    public void OnButtonSelected()
    {
        var otherNav = downSelectable.navigation;
        otherNav.selectOnUp = Button;
        downSelectable.navigation = otherNav;
    }

    public void SetNextNav(Selectable selectable)
    {
        var nav = Button.navigation;
        nav.selectOnRight = selectable;
        Button.navigation = nav;
    }
    
    public void SetPreviousNav(Selectable selectable)
    {
        var nav = Button.navigation;
        nav.selectOnLeft = selectable;
        Button.navigation = nav;
    }

    public void InitButton(int id,Selectable selectable)
    {
        levelId = id; //0 is level 1
        
        levelText.text = $"level {levelId+1}";

        gameObject.name = $"Selectable Level {levelId+1}";
        
        UpdateSelectable(selectable);
        
        UpdateLevelHighscore();
        
        UpdateHeight();
    }

    private void UpdateSelectable(Selectable selectable)
    {
        downSelectable = selectable;
        
        var selfNav = Button.navigation;
        selfNav.selectOnDown = downSelectable;
        Button.navigation = selfNav;
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
        
        highScoreText.text = highscore >= 0 ? $"record : {LevelController.ScoreToText(highscore)}" : "no record";
    }

    private void LaunchLevel()
    {
        LevelTracker.LaunchLevel(levelId);
    }

    public (Vector3 leftPos, Vector3 rightPos) GetPositions()
    {
        var tr = GetComponent<RectTransform>();
        var size = levelTr.sizeDelta;
        var left = tr.localPosition;
        left.y += levelTr.localPosition.y;
        left.z = 1;
        var right = left;

        right.x += size.x;

        return (left, right);
    }
}
