using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSelector : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform rectTransform;
    private Vector2 expectedSize;
    
    public void OnSelect(BaseEventData eventData)
    {
        text.color = UISettingsSo.CurrentSettings.Dark;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = UISettingsSo.CurrentSettings.White;
    }

    private void OnDisable()
    {
        rectTransform.DOKill();
    }
}
