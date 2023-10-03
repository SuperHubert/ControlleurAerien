using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SelectableLevel selectableLevelPrefab;
    [SerializeField] private Transform selectableLevelParent;
    
    private void Start()
    {
        var levels = LevelTracker.AvailableLevelCount;

        for (int i = 0; i < levels; i++)
        {
            var level = Instantiate(selectableLevelPrefab, selectableLevelParent);
            level.InitButton(i);
        }
    }

    
}
