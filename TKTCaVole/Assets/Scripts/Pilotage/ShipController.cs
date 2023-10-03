using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShipController : MonoBehaviour
{
    
    private Vector2 moveVector = Vector2.zero;
     
    public float speed = 100.0f;
    public int gear = 0;
    public int gearValue = 20;
    public float rotationSpeed = 75.0f;


    public TextMeshProUGUI gearText;

    [Header("Anim FOV")]
    private float animDuration = 1f;
    [SerializeField]
    private AnimationCurve curve;


    private void Start()
    {
        GameInputManager.OnMovementPerformed += OnMovementPerformed;
        GameInputManager.OnMovementCancelled += OnMovementCancelled;
        GameInputManager.OnGearUpPerformed += OnGearUpPerformed;
        GameInputManager.OnGearDownPerformed += OnGearDownPerformed;
    }

    private void OnGearUpPerformed(InputAction.CallbackContext obj)
    {
        if (gear < 5)
        {
            gear++; //changer le FOV
            StopAllCoroutines();
            StartCoroutine(LerpFOV(gear, animDuration));
        }
        gearText.text = gear.ToString();
    }

    private void OnGearDownPerformed(InputAction.CallbackContext obj)
    {
        if (gear > 0)
        {
            gear--;
            StopAllCoroutines();
            StartCoroutine(LerpFOV(gear, animDuration)); //changer le FOV
        }
        gearText.text = gear.ToString();
    }

    private IEnumerator LerpFOV(int gear, float duration)
    {
        float timeElapsed = 0f;
        Camera cam = Camera.main;
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

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = speed + gear*gearValue;
        transform.Translate(0, 0, currentSpeed * Time.deltaTime);
        
        float rotationSpeedVertical = moveVector.y * rotationSpeed * Time.deltaTime;
        float rotationSpeedHorizontal = moveVector.x * rotationSpeed * Time.deltaTime;
        
        transform.Rotate(rotationSpeedVertical, 0,rotationSpeedHorizontal);
        /*if (Input.GetKeyDown(KeyCode.E) && gear < 5) gear++; //changer le FOV
        if (Input.GetKeyDown(KeyCode.A) && gear > 0) gear--;*/
    }

    private void OnMovementPerformed(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
        
        
    }

    private void OnMovementCancelled(InputAction.CallbackContext ctx)
    {
        moveVector = Vector2.zero;
        
    }

}
