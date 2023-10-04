using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class ScriptableSetting : ScriptableObject
{
    public float cameraSensitivity;

    public bool invertCameraY;
    public bool invertShipY;
    public bool invertShipX;
}
