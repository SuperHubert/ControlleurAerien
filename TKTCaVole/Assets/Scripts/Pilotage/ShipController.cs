using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ShipController : MonoBehaviour
{
    
    private Vector2 moveVector = Vector2.zero;
     
    public float speed = 100.0F;
    public int gear = 0;
    public int gearValue = 20;
    public float rotationSpeed = 75.0F;

    private void Start()
    {
        GameInputManager.OnMovementPerformed += OnMovementPerformed;
        GameInputManager.OnMovementCancelled += OnMovementCancelled;
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = speed + gear*gearValue;
        transform.Translate(0, 0, currentSpeed * Time.deltaTime);
        
        float rotationSpeedVertical = moveVector.y * rotationSpeed * Time.deltaTime;
        float rotationSpeedHorizontal = moveVector.x * rotationSpeed * Time.deltaTime;
        
        transform.Rotate(rotationSpeedVertical, rotationSpeedHorizontal, 0);
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
