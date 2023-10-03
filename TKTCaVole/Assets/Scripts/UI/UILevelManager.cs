using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] private SelectableLevel selectableLevelPrefab;
    [SerializeField] private Transform selectableLevelParent;
    [SerializeField] private ScrollRect scrollRect; 
    
    private void Start()
    {
        var levels = LevelTracker.AvailableLevelCount;
        //var levels = 150;

        for (int i = 0; i < levels; i++)
        {
            var level = Instantiate(selectableLevelPrefab, selectableLevelParent);
            level.InitButton(i);
        }
    }
    
    public void ScrollToLastLevel()
    {
        StartCoroutine(DelayedScroll());
        
        IEnumerator DelayedScroll()
        {
            yield return null;
            scrollRect.horizontalNormalizedPosition = 1f;
        }
    }


}
