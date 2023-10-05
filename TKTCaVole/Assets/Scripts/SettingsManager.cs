using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private ScriptableSetting settings;
    [SerializeField] private GameObject panel;
    [SerializeField] private float transitionDuration = 0.05f;
    
    [Header("Components")]
    [SerializeField] private Slider mouseScrollbar;
    [SerializeField] private TextMeshProUGUI sensitivityFeedbacktext;
    [SerializeField] private Toggle cameraYToggle;
    [SerializeField] private Toggle cameraXToggle;
    [SerializeField] private Toggle shipXToggle;
    [SerializeField] private Toggle shipYToggle;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;

    private string sensitivityKey = "MouseSensitivity";
    private string cameraYKey = "cameraY";
    private string cameraXKey = "cameraX";
    private string shipXKey = "shipX";
    private string shipYKey = "shipY";

    private Selectable returnSelectable;
    private Action extra;
    
    private void Start()
    {
        Close(true);
        
        LoadSettings();
        
        ApplySettings();
        
        mouseScrollbar.onValueChanged.AddListener(UpdateSensitivity);
        cameraYToggle.onValueChanged.AddListener(UpdateCameraY);
        cameraXToggle.onValueChanged.AddListener(UpdateCameraX);
        shipXToggle.onValueChanged.AddListener(UpdateShipX);
        shipYToggle.onValueChanged.AddListener(UpdateShipY);
        
        saveButton.onClick.AddListener(Save);
        closeButton.onClick.AddListener(ClosePanel);
        return;

        void ClosePanel()
        {
            Close();
        }
    }

    

    private void ApplySettings()
    {
        mouseScrollbar.value = settings.cameraSensitivity;
        sensitivityFeedbacktext.text = $"{mouseScrollbar.value:00}";
        cameraYToggle.isOn = settings.invertCameraY;
        cameraXToggle.isOn = settings.invertCameraX;
        shipXToggle.isOn = settings.invertShipX;
        shipYToggle.isOn = settings.invertShipY;
    }

    private void LoadSettings()
    {
        if(!PlayerPrefs.HasKey(sensitivityKey)) PlayerPrefs.SetFloat(sensitivityKey,settings.cameraSensitivity);
        if(!PlayerPrefs.HasKey(cameraYKey)) PlayerPrefs.SetInt(cameraYKey,settings.invertCameraY ? 1 : 0);
        if(!PlayerPrefs.HasKey(cameraXKey)) PlayerPrefs.SetInt(cameraXKey,settings.invertCameraX ? 1 : 0);
        if(!PlayerPrefs.HasKey(shipXKey)) PlayerPrefs.SetInt(shipYKey,settings.invertShipX ? 1 : 0);
        if(!PlayerPrefs.HasKey(shipYKey)) PlayerPrefs.SetInt(shipYKey,settings.invertShipY ? 1 : 0);

        settings.cameraSensitivity = PlayerPrefs.GetFloat(sensitivityKey);
        settings.invertCameraX = PlayerPrefs.GetInt(cameraXKey) == 1;
        settings.invertCameraY = PlayerPrefs.GetInt(cameraYKey) == 1;
        settings.invertShipX = PlayerPrefs.GetInt(shipXKey) == 1;
        settings.invertShipY = PlayerPrefs.GetInt(shipYKey) == 1;
    }

    [ContextMenu("Reset Prefs")]
    private void ResetSettings()
    {
        PlayerPrefs.SetFloat(sensitivityKey,settings.cameraSensitivity);
        PlayerPrefs.SetInt(cameraYKey,settings.invertCameraY ? 1 : 0);
        PlayerPrefs.SetInt(cameraXKey,settings.invertCameraX ? 1 : 0);
        PlayerPrefs.SetInt(shipYKey,settings.invertShipX ? 1 : 0);
        PlayerPrefs.SetInt(shipYKey,settings.invertShipY ? 1 : 0);
    }

    private void UpdateSensitivity(float value)
    {
        sensitivityFeedbacktext.text = $"{value}";

        settings.cameraSensitivity = value;
        
        PlayerPrefs.SetFloat(sensitivityKey,value);
    }

    private void UpdateCameraY(bool value)
    {
        settings.invertCameraY = value;
        
        PlayerPrefs.SetInt(cameraYKey,value ? 1 : 0);
    }
    private void UpdateCameraX(bool value)
    {
        settings.invertCameraX = value;
        PlayerPrefs.SetInt(cameraXKey,value ? 1 : 0);
    }
    private void UpdateShipX(bool value)
    {
        settings.invertShipX = value;
        
        PlayerPrefs.SetInt(shipXKey,value ? 1 : 0);
    }
    
    private void UpdateShipY(bool value)
    {
        settings.invertShipY = value;
        
        PlayerPrefs.SetInt(shipYKey,value ? 1 : 0);
    }
    private void Save()
    {
        PlayerPrefs.Save();
        ApplySettings();
        Close();
    }

    public void Open(Selectable selectable,Action callback = null)
    {
        panel.SetActive(true);
        var tr = panel.transform;

        tr.localScale = new Vector3(1, 0, 1);
        tr.DOScaleY(1, transitionDuration).SetUpdate(true);
        
        returnSelectable = selectable;
        
        closeButton.Select();
        saveButton.Select();

        extra = callback;
        
        ApplySettings();
    }

    private void Close(bool skipAnimation = false)
    {
        if (skipAnimation)
        {
            Skip();
            return;
        }
        
        var tr = panel.transform;

        tr.localScale = new Vector3(1, 1, 1);
        tr.DOScaleY(0, transitionDuration).OnComplete(Skip).SetUpdate(true);
        
        return;
        
        void Skip()
        {
            panel.SetActive(false);
        
            extra?.Invoke();
        
            if(returnSelectable == null) return;
            returnSelectable.Select();
        }
    }
}
