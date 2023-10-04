using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScreenShake : MonoBehaviour
{
    public static Action<float, float> OnCameraShake;

    private void Start()
    {
        OnCameraShake = null;
        OnCameraShake += Shake;
    }

    public void Shake(float duration, float magnitude)
    {
        transform.DOShakePosition(duration, magnitude, 10, 90, false, true);
    }
}
