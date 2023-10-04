using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShipController : MonoBehaviour
{
    [SerializeField] private ScriptableSetting settings;
        
    private Vector2 moveVector = Vector2.zero;

    private bool invertXAxis => settings.invertShipX;
    private bool invertYAxis => settings.invertShipY;
    [Header("Movement")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float speed = 10f, rotationSpeed = 75.0f;
    [SerializeField]
    private int gear = 0, gearValue = 2;

    public event Action<int> OnGearChanged; 
    
    private void Start()
    {
        GameInputManager.OnMovementPerformed += OnMovementPerformed;
        GameInputManager.OnMovementCancelled += OnMovementCancelled;
        GameInputManager.OnGearUpPerformed += OnGearUpPerformed;
        GameInputManager.OnGearDownPerformed += OnGearDownPerformed;
    }
    

    private void OnGearUpPerformed(InputAction.CallbackContext obj)
    {
        if (gear >= 5) return;
        
        gear++; //changer le FOV
            
        OnGearChanged?.Invoke(gear);
    }

    private void OnGearDownPerformed(InputAction.CallbackContext obj)
    {
        if (gear <= 0) return;
        
        gear--;
            
        OnGearChanged?.Invoke(gear);
    }

    void FixedUpdate()
    {

        float currentSpeed = speed + gear * gearValue;
        if (rb.velocity.magnitude < currentSpeed)
        {
            rb.AddForce(transform.forward * currentSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        rb.AddTorque(transform.right * moveVector.y * rotationSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        rb.AddTorque(transform.forward * moveVector.x * rotationSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);

        /*float currentSpeed = speed + gear*gearValue;
        transform.Translate(0, 0, currentSpeed * Time.deltaTime);

        
        
        float rotationSpeedVertical = moveVector.y * rotationSpeed * Time.deltaTime;
        float rotationSpeedHorizontal = moveVector.x * rotationSpeed * Time.deltaTime;
        
        transform.Rotate(rotationSpeedVertical, 0,rotationSpeedHorizontal);*/

    }

    private void OnMovementPerformed(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
        moveVector = moveVector.normalized;
        if (invertXAxis) moveVector.x *= -1;
        if (invertYAxis) moveVector.y *= -1;
        
        
    }

    private void OnMovementCancelled(InputAction.CallbackContext ctx)
    {
        moveVector = Vector2.zero;
        
    }

}
