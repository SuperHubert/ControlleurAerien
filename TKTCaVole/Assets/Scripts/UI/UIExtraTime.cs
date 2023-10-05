using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExtraTime : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private List<Graphic> graphics = new();
    [SerializeField] private RectTransform tr;

    [Header("Settings")]
    [SerializeField] private float height = 45f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float fadeDuration = 2f;
    
    private Color color;

    public void Init()
    {
        tr.localPosition = Vector3.zero;
        color = text.color; 
        color.a = 0;
        foreach (var graphic in graphics)
        {
            graphic.color = color;
        }
        color.a = 1;
    }
    
    public void PlayAnim(float time)
    {
        foreach (var graphic in graphics)
        {
            graphic.color = color;
        }

        text.text = $"{LevelController.ScoreToText(time)}";

        tr.DOLocalMoveY(height, moveDuration);

        foreach (var graphic in graphics)
        {
            graphic.DOFade(0, fadeDuration);
        }
        
        
    }

    private void OnDisable()
    {
        text.DOKill();
    }
}
