using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSelector : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private string SelectAudioKey;
    [SerializeField] private string ConfirmAudioKey;
    private Vector2 expectedSize;

    public void OnSelect(BaseEventData eventData)
    {
        text.color = UISettingsSo.CurrentSettings.Dark;
        AudioManager.Instance.PlaySound(SelectAudioKey);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = UISettingsSo.CurrentSettings.White;
    }

    private void OnDisable()
    {
        rectTransform.DOKill();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (AudioManager.Instance)
            AudioManager.Instance.PlaySound(ConfirmAudioKey);
    }
}