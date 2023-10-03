using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableLevel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    private GameObject HighScoreGo => highScoreText.gameObject;
    [SerializeField] private RectTransform levelTr;
    
    [Header("Data")]
    [SerializeField] private int levelId;

    private void Start()
    {
        button.onClick.AddListener(LaunchLevel);
    }

    public void InitButton(int id)
    {
        levelId = id; //0 is level 1
        
        levelText.text = $"Level {levelId+1}";
        
        UpdateLevelHighscore();
        
        //TODO - Update level height;
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
