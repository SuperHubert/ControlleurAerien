using System;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Settings")]
public class UISettingsSo : ScriptableObject
{
    [field:SerializeField] public Color White { get; private set; }
    [field:SerializeField] public Color Dark { get; private set; }
    
    public static UISettingsSo CurrentSettings { get; private set; }
    
    public void SetInstance()
    {
        CurrentSettings = this;
    }
}
