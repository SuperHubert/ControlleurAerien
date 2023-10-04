using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIShip : MonoBehaviour
{
    [SerializeField] private ShipController controller;
    
    [SerializeField] private TextMeshProUGUI gearText;
    [SerializeField] private Camera cam;
    
    [Header("Anim FOV")]
    private float animDuration = 1f;
    [SerializeField]
    private AnimationCurve curve;
    private Coroutine fovRoutine;
    

    
    [Header("Pause")]
    private bool isGamePaused = false;

    [SerializeField] private GameObject pausePanel;
    private void Start()
    {
        controller.OnGearChanged += UpdateGearText;
        GameInputManager.OnPausePerformed += OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        isGamePaused = !isGamePaused; 
        pausePanel.SetActive(isGamePaused);
        Time.timeScale = isGamePaused?0:1;
    }

    private void UpdateGearText(int gear)
    {
        gearText.text = $"{gear}";

        cam.DOKill();
        cam.DOFieldOfView(40 + gear * 5, animDuration).SetEase(curve);
        
        /*
        if(fovRoutine != null) StopCoroutine(fovRoutine);
        fovRoutine = StartCoroutine(LerpFOV(gear, animDuration)); //changer le FOV
        */
    }
    
    private IEnumerator LerpFOV(int gear, float duration)
    {
        float timeElapsed = 0f;
        print(cam);
        float currFov = cam.fieldOfView;
        while (timeElapsed < duration)
        {
            cam.fieldOfView = Mathf.Lerp(currFov, 40 + gear * 5, curve.Evaluate(timeElapsed / duration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        cam.fieldOfView = 40 + gear * 5;

        yield return null;
    }
}
