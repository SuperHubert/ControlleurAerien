using System;
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
    [SerializeField] private LineRenderer lineRenderer;
    
    private void Start()
    {
        var levels = LevelTracker.AvailableLevelCount;
        selectableLevels.Clear();
        //var levels = 150;

        SelectableLevel previousCreated = null;
        for (int i = 0; i < levels; i++)
        {
            var createdLevel = Instantiate(selectableLevelPrefab, selectableLevelParent);
            if (previousCreated != null)
            {
                previousCreated.SetNextNav(createdLevel.Button);
                createdLevel.SetPreviousNav(previousCreated.Button);
            }
            
            createdLevel.InitButton(i,downSelectable);
            selectableLevels.Add(createdLevel);

            previousCreated = createdLevel;
        }
    }

    public void UpdateLineRenderer()
    {
        lineRenderer.startColor = UISettingsSo.CurrentSettings.White;
        lineRenderer.endColor = UISettingsSo.CurrentSettings.White;
        lineRenderer.positionCount = selectableLevels.Count * 2;
        for (var index = 0; index < selectableLevels.Count; index++)
        {
            var selectable = selectableLevels[index];
            var points = selectable.GetPositions();
            
            lineRenderer.SetPosition(2*index,points.leftPos);
            lineRenderer.SetPosition(2*index+1,points.rightPos);
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
