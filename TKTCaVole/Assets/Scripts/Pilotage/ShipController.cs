using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShipController : MonoBehaviour
{
    [SerializeField] private ScriptableSetting settings;
    [SerializeField] private Collider col;
        
    private Vector2 moveVector = Vector2.zero;

    private bool invertXAxis => settings.invertShipX;
    private bool invertYAxis => settings.invertShipY;
    [Header("Movement")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float speed = 10f, rotationSpeed = 75.0f, clockRotSpeed = 2f;
    [SerializeField]
    private int gear = 0, gearValue = 2;
    
    public event Action<int> OnGearChanged;

    
    private bool rotateClockwise, rotateCounterClockwise;
    
    private void Start()
    {
        GameInputManager.OnMovementPerformed += OnMovementPerformed;
        GameInputManager.OnMovementCancelled += OnMovementCancelled;
        GameInputManager.OnGearUpPerformed += OnGearUpPerformed;
        GameInputManager.OnGearUpCancelled += OnGearUpCancelled;
        GameInputManager.OnGearDownPerformed += OnGearDownPerformed;
        GameInputManager.OnGearDownCancelled += OnGearDownCancelled;

        LevelController.OnLevelEnd += OnLevelEnd;
        
        col.enabled = true;
    }

    private void OnDisable()
    {
        GameInputManager.OnMovementPerformed -= OnMovementPerformed;
        GameInputManager.OnMovementCancelled -= OnMovementCancelled;
        GameInputManager.OnGearUpPerformed -= OnGearUpPerformed;
        GameInputManager.OnGearUpCancelled -= OnGearUpCancelled;
        GameInputManager.OnGearDownPerformed -= OnGearDownPerformed;
        GameInputManager.OnGearDownCancelled -= OnGearDownCancelled;

        LevelController.OnLevelEnd -= OnLevelEnd;
    }

    private void OnLevelEnd(bool _,float __)
    {
        col.enabled = false;
    }


    private void OnGearUpPerformed(InputAction.CallbackContext obj)
    {
        rotateClockwise = true;

    }
    private void OnGearUpCancelled(InputAction.CallbackContext obj)
    {
        rotateClockwise = false;
    }
    private void OnGearDownPerformed(InputAction.CallbackContext obj)
    {
        rotateCounterClockwise = true;
    }
    private void OnGearDownCancelled(InputAction.CallbackContext obj)
    {
        rotateCounterClockwise = false;
    }
    
    void FixedUpdate()
    {

        float currentSpeed = speed + gear * gearValue;
        if (rb.velocity.magnitude < currentSpeed)
        {
            rb.AddForce(transform.forward * (currentSpeed * Time.deltaTime), ForceMode.Impulse);
        }
        rb.AddTorque(transform.right * (moveVector.y * rotationSpeed * Time.deltaTime), ForceMode.Acceleration);
        rb.AddTorque(transform.up * (moveVector.x * rotationSpeed * Time.deltaTime), ForceMode.Acceleration);
        if(rotateClockwise) rb.AddTorque(transform.forward * (-clockRotSpeed*rotationSpeed * Time.deltaTime), ForceMode.Acceleration);
        else if(rotateCounterClockwise) rb.AddTorque(transform.forward * (clockRotSpeed*rotationSpeed * Time.deltaTime), ForceMode.Acceleration);

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
