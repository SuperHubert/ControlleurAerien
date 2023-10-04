using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] private SelectableLevel selectableLevelPrefab;
    private List<SelectableLevel> selectableLevels = new();
    [SerializeField] private Transform selectableLevelParent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Selectable downSelectable;
    
    private void Start()
    {
        var levels = LevelTracker.AvailableLevelCount;
        selectableLevels.Clear();
        //var levels = 150;

        for (int i = 0; i < levels; i++)
        {
            var level = Instantiate(selectableLevelPrefab, selectableLevelParent);
            level.InitButton(i,downSelectable);
            selectableLevels.Add(level);
        }
    }
    
    public void ScrollToLastLevel()
    {
        StartCoroutine(DelayedScroll());
        
        return;

        IEnumerator DelayedScroll()
        {
            yield return null;
            scrollRect.horizontalNormalizedPosition = 1f;
            selectableLevels[^1].Select();
        }
    }


}
