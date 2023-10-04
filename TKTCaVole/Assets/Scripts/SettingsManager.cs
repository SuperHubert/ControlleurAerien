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
    [SerializeField] private Scrollbar mouseScrollbar;
    [SerializeField] private TextMeshProUGUI sensitivityFeedbacktext;
    [SerializeField] private Toggle cameraYToggle;
    [SerializeField] private Toggle shipXToggle;
    [SerializeField] private Toggle shipYToggle;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;

    private string sensitivityKey = "MouseSensitivity";
    private string cameraYKey = "cameraY";
    private string shipXKey = "shipX";
    private string shipYKey = "shipY";

    private Selectable returnSelectable;
    
    private void Start()
    {
        Close();
        
        LoadSettings();
        
        ApplySettings();
        
        mouseScrollbar.onValueChanged.AddListener(UpdateSensitivity);
        cameraYToggle.onValueChanged.AddListener(UpdateCameraY);
        shipXToggle.onValueChanged.AddListener(UpdateShipX);
        shipYToggle.onValueChanged.AddListener(UpdateShipY);
        
        saveButton.onClick.AddListener(Save);
        closeButton.onClick.AddListener(Close);
    }

    private void ApplySettings()
    {
        mouseScrollbar.value = settings.cameraSensitivity;
        sensitivityFeedbacktext.text = $"{mouseScrollbar.value}";
        cameraYToggle.isOn = settings.invertCameraY;
        shipXToggle.isOn = settings.invertShipX;
        shipYToggle.isOn = settings.invertShipY;
    }

    private void LoadSettings()
    {
        if(!PlayerPrefs.HasKey(sensitivityKey)) PlayerPrefs.SetFloat(sensitivityKey,settings.cameraSensitivity);
        if(!PlayerPrefs.HasKey(cameraYKey)) PlayerPrefs.SetInt(cameraYKey,settings.invertCameraY ? 1 : 0);
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
        
        PlayerPrefs.SetFloat(sensitivityKey,value);
    }

    private void UpdateCameraY(bool value)
    {
        settings.invertCameraY = value;
        
        PlayerPrefs.SetInt(cameraYKey,value ? 1 : 0);
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
    }

    public void Open(Selectable selectable)
    {
        panel.SetActive(true);

        returnSelectable = selectable;
        
        saveButton.Select();
        
        ApplySettings();
    }

    private void Close()
    {
        panel.SetActive(false);
        
        if(returnSelectable == null) return;
        returnSelectable.Select();
    }
}
