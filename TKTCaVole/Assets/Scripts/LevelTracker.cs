using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour
{
    private static List<int> levelHighscoreTracker = new ();
    public static int AvailableLevelCount => levelHighscoreTracker.Count;
    public static int CurrentLevel { get; private set; } //0 is level 1;
    
    private void Awake()
    {
        levelHighscoreTracker.Clear();

        GetCompletedLevels();

        GetLevelHighScores();
    }

    private void GetCompletedLevels()
    {
        if (PlayerPrefs.HasKey("AvailableLevels"))
        {
            var completedLevels = PlayerPrefs.GetInt("AvailableLevels");

            if (completedLevels <= 0)
            {
                IncreaseAvailableLevels();
                return;
            }
            
            for (int i = 0; i < completedLevels; i++)
            {
                levelHighscoreTracker.Add(0);
            }
            return;
        }
        
        IncreaseAvailableLevels();
    }

    private void GetLevelHighScores()
    {
        for (int i = 0; i < levelHighscoreTracker.Count; i++)
        {
            levelHighscoreTracker[i] = GetLevelHighscore(i);
        }
    }

    public static int GetLevelHighscore(int index)
    {
        if (index < 0 || index > levelHighscoreTracker.Count)
        {
            return -2;
        }

        if (!PlayerPrefs.HasKey($"Level{index}")) return -1;
        
        var highscore = PlayerPrefs.GetInt($"Level{index}");
        return highscore;
    }

    public static void LaunchLevel(int index)
    {
        Debug.Log($"Launching level {index}");
        
        CurrentLevel = index;
        
        SceneManager.LoadScene(1);
    }

    public static void CompleteLevel(int score)
    {
        if (PlayerPrefs.HasKey($"Level{CurrentLevel}"))
        {
            var highscore = PlayerPrefs.GetInt($"Level{CurrentLevel}");
            
            if(score < highscore) return;
        }
        
        PlayerPrefs.SetInt($"Level{CurrentLevel}",score);

        if (CurrentLevel == levelHighscoreTracker.Count - 1)
        {
            IncreaseAvailableLevels();
        }
        
        SceneManager.LoadScene(0);
    }

    private static void IncreaseAvailableLevels()
    {
        levelHighscoreTracker.Add(0);
        PlayerPrefs.SetInt("AvailableLevels",levelHighscoreTracker.Count);
        
        PlayerPrefs.SetInt($"Level{levelHighscoreTracker.Count-1}",0);
    }

    [ContextMenu("Reset Progress")]
    private void ResetProgress()
    {
        PlayerPrefs.SetInt("AvailableLevels",0);
    }
}
