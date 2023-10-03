using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour
{
    [SerializeField] private bool showLogs;
    private static bool log;
    [SerializeField] private List<float> debug;
    private static List<float> levelHighscoreTracker = new ();
    public static int AvailableLevelCount => levelHighscoreTracker.Count;
    public static int CurrentLevel { get; private set; } //0 is level 1;
    
    private void Awake()
    {
        debug = levelHighscoreTracker;
        log = showLogs;
        
        levelHighscoreTracker.Clear();

        GetCompletedLevels();

        GetLevelHighScores();
    }

    private void GetCompletedLevels()
    {
        if (PlayerPrefs.HasKey("AvailableLevels"))
        {
            var completedLevels = PlayerPrefs.GetFloat("AvailableLevels");

            Log($"Found {completedLevels} available Levels");
            
            if (completedLevels <= 0)
            {
                IncreaseAvailableLevels();
                return;
            }
            
            for (int i = 0; i < completedLevels; i++)
            {
                levelHighscoreTracker.Add(-1);
            }
            return;
        }
        
        IncreaseAvailableLevels();
    }

    private void GetLevelHighScores()
    {
        for (int i = 0; i < levelHighscoreTracker.Count; i++)
        {
            levelHighscoreTracker[i] = GetLevelHighscoreFromPlayerPrefs(i);
        }
    }

    public static float GetLevelHighscoreFromPlayerPrefs(int index)
    {
        if (index < 0 || index >= levelHighscoreTracker.Count)
        {
            return -2;
        }

        if (!PlayerPrefs.HasKey($"Level{index}")) return -1;
        
        var highscore = PlayerPrefs.GetFloat($"Level{index}");
        return highscore;
    }

    public static float GetLevelHighscore(int index)
    {
        if (index < 0 || index >= levelHighscoreTracker.Count)
        {
            return -1;
        }

        return levelHighscoreTracker[index];
    }

    public static void LaunchLevel(int index)
    {
        Log($"Launching level {index}");
        
        CurrentLevel = index;
        
        SceneManager.LoadScene(1);
    }

    public static void CompleteLevel(float score)
    {
        Log($"Completing level {CurrentLevel} with score of {score}");
        
        var key = $"Level{CurrentLevel}";
        if (PlayerPrefs.HasKey($"Level{CurrentLevel}"))
        {
            var highscore = PlayerPrefs.GetFloat(key);
            
            Log($"Has player pref '{key}', with score of {highscore}");
            
            if(score > highscore && highscore >= 0) return; //score is time, so lower is better
        }
        
        PlayerPrefs.SetFloat(key,score);
        
        Log($"Saved player pref '{key}', with score of {score}");
        
        if (CurrentLevel == levelHighscoreTracker.Count - 1)
        {
            IncreaseAvailableLevels();
        }
        
        SceneManager.LoadScene(0);
    }

    private static void IncreaseAvailableLevels()
    {
        levelHighscoreTracker.Add(-1);
        PlayerPrefs.SetFloat("AvailableLevels",levelHighscoreTracker.Count);

        var index = levelHighscoreTracker.Count - 1;
        PlayerPrefs.SetFloat($"Level{index}",levelHighscoreTracker[index]);
    }

    [ContextMenu("Reset Progress")]
    private void ResetProgress()
    {
        PlayerPrefs.SetFloat("AvailableLevels",0);
    }

    private static void Log(string str)
    {
        if(!log) return;
        Debug.Log(str);
    }
}
