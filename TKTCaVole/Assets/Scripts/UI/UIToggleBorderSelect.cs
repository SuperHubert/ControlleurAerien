using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIToggleBorderSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject borderObj;
    [SerializeField] private List<Graphic> graphics = new();

    private void Start()
    {
        borderObj.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        borderObj.SetActive(true);
        var col = UISettingsSo.CurrentSettings.Dark;
        foreach (var graphic in graphics)
        {
            graphic.color = col;
        }
    }


    public void OnDeselect(BaseEventData eventData)
    {
        borderObj.SetActive(false);
        var col = UISettingsSo.CurrentSettings.White;
        foreach (var graphic in graphics)
        {
            graphic.color = col;
        }
    }
}
