using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    private ShipInput input = null;
    
    public static event Action<InputAction.CallbackContext> OnMovementPerformed;
    public static event Action<InputAction.CallbackContext> OnMovementCancelled;
    
    public static event Action<InputAction.CallbackContext> OnCameraPerformed;
    public static event Action<InputAction.CallbackContext> OnCameraCancelled;
    
    
    
    
    void Awake()
    {
        input = new ShipInput();
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += InvokeMovement;
        input.Player.Movement.canceled += InvokeCancel;

        input.Player.Camera.performed += InvokeCamMovement;
        input.Player.Camera.canceled += InvokeCamCancel;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= InvokeMovement;
        input.Player.Movement.canceled -= InvokeCancel;
        
        input.Player.Camera.performed -= InvokeCamMovement;
        input.Player.Camera.canceled -= InvokeCamCancel;
    }

    private void InvokeMovement(InputAction.CallbackContext ctx)
    {
        OnMovementPerformed?.Invoke(ctx);
    }
    private void InvokeCancel(InputAction.CallbackContext ctx)
    {
        OnMovementCancelled?.Invoke(ctx);
    }


    private void InvokeCamMovement(InputAction.CallbackContext ctx)
    {
        OnCameraPerformed?.Invoke(ctx);
    }
    private void InvokeCamCancel(InputAction.CallbackContext ctx)
    {
        OnCameraCancelled?.Invoke(ctx);
    }


    
    
}
