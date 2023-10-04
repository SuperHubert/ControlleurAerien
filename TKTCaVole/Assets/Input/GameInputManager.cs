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
    public static event Action<InputAction.CallbackContext> OnCameraCenter;
    
    public static event Action<InputAction.CallbackContext> OnGearUpPerformed;
    public static event Action<InputAction.CallbackContext> OnGearDownPerformed;

    public static event Action<InputAction.CallbackContext> OnSecondaryShootPerformed;
    public static event Action<InputAction.CallbackContext> OnPrimaryShootPerformed;

    public static event Action<InputAction.CallbackContext> OnPausePerformed;
    
    
    
    void Awake()
    {
        input = new ShipInput();
    }

    void OnEnable()
    {
        OnMovementPerformed = null;
        OnMovementCancelled = null;
        OnCameraPerformed = null;
        OnCameraCancelled= null;
        OnGearUpPerformed= null;
        OnGearDownPerformed= null;
        OnSecondaryShootPerformed = null;
        OnPrimaryShootPerformed= null;
        OnPausePerformed= null;
        OnCameraCenter = null;
        
        input.Enable();
        input.Player.Movement.performed += InvokeMovement;
        input.Player.Movement.canceled += InvokeCancel;

        input.Player.Camera.performed += InvokeCamMovement;
        input.Player.Camera.canceled += InvokeCamCancel;
        input.Player.CenterCam.performed += InvokeCamCentre;

        input.Player.GearUp.performed += InvokeGearUp;
        input.Player.GearDown.performed += InvokeGearDown;
        input.Player.SecondaryWeaponShoot.performed += InvokeSecondaryShoot;
        input.Player.PrimaryWeaponShoot.performed += InvokePrimaryShoot;

        input.Player.Pause.performed += InvokePause;
    }

    

    public static void InvokePause(InputAction.CallbackContext ctx)
    {
        OnPausePerformed?.Invoke(ctx);
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= InvokeMovement;
        input.Player.Movement.canceled -= InvokeCancel;
        
        input.Player.Camera.performed -= InvokeCamMovement;
        input.Player.Camera.canceled -= InvokeCamCancel;
        input.Player.CenterCam.performed -= InvokeCamCentre;
        
        input.Player.GearUp.performed -= InvokeGearUp;
        input.Player.GearDown.performed -= InvokeGearDown;
        input.Player.SecondaryWeaponShoot.performed -= InvokeSecondaryShoot;
        input.Player.PrimaryWeaponShoot.performed -= InvokePrimaryShoot;

        input.Player.Pause.performed -= InvokePause;
        
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
    private void InvokeCamCentre(InputAction.CallbackContext ctx)
    {
        OnCameraCenter?.Invoke(ctx);
    }

    private void InvokeGearUp(InputAction.CallbackContext ctx)
    {
        OnGearUpPerformed?.Invoke(ctx);
    }
    private void InvokeGearDown(InputAction.CallbackContext ctx)
    {
        OnGearDownPerformed?.Invoke(ctx);
    }
    private void InvokeSecondaryShoot(InputAction.CallbackContext ctx)
    {
        OnSecondaryShootPerformed?.Invoke(ctx);
    }
    private void InvokePrimaryShoot(InputAction.CallbackContext ctx)
    {
        OnPrimaryShootPerformed?.Invoke(ctx);
    }




}
