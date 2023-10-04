using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private ScriptableSetting settings;
    [SerializeField] private GameObject panel;
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
        Close();
        
        LoadSettings();
        
        ApplySettings();
        
        mouseScrollbar.onValueChanged.AddListener(UpdateSensitivity);
        cameraYToggle.onValueChanged.AddListener(UpdateCameraY);
        cameraXToggle.onValueChanged.AddListener(UpdateCameraX);
        shipXToggle.onValueChanged.AddListener(UpdateShipX);
        shipYToggle.onValueChanged.AddListener(UpdateShipY);
        
        saveButton.onClick.AddListener(Save);
        closeButton.onClick.AddListener(Close);
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
        settings.invertCameraY = PlayerPrefs.GetInt(cameraYKey) == 1;
        settings.invertShipX = PlayerPrefs.GetInt(shipXKey) == 1;
        settings.invertShipY = PlayerPrefs.GetInt(shipYKey) == 1;
    }

    [ContextMenu("Reset Prefs")]
    private void ResetSettings()
    {
        PlayerPrefs.SetFloat(sensitivityKey,0);
        PlayerPrefs.SetInt(cameraYKey,0);
        PlayerPrefs.SetInt(shipYKey,0);
        PlayerPrefs.SetInt(shipYKey,0);
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

        returnSelectable = selectable;
        
        closeButton.Select();
        saveButton.Select();

        extra = callback;
        
        ApplySettings();
    }

    private void Close()
    {
        panel.SetActive(false);
        
        extra?.Invoke();
        
        if(returnSelectable == null) return;
        returnSelectable.Select();
    }
}
