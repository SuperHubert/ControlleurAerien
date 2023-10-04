using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class ScriptableSetting : ScriptableObject
{
    public float cameraSensitivity = 70f;

    public bool invertCameraY;
    public bool invertCameraX;
    public bool invertShipY;
    public bool invertShipX;
}
