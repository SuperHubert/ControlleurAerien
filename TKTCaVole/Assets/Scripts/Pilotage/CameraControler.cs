using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform spaceShip,child;
    [SerializeField] private ScriptableSetting settings;
    private Vector2 moveVector = Vector2.zero;
    
    private bool invertXAxis => settings.invertCameraX;
    private bool invertYAxis => settings.invertCameraY;

    private Vector3 _currentCameraRot = new Vector3(0, 0, -0);
    
    private float MouseSensitivity => settings.cameraSensitivity;
    
    private bool doCenter = false;
    private float animDuration = 0.1f;
    private float timeElapsed = .0f;
    private Vector3 instantTCamPos;

    private bool gameEnded = false;

    
    
    private void Start()
    {
        GameInputManager.OnCameraPerformed += OnCamMovementPerformed;
        GameInputManager.OnCameraCancelled += OnCamMovementCancelled;
        GameInputManager.OnCameraCenter += OnCameraCenter;

        LevelController.OnLevelEnd += Disable;

        gameEnded = false;
        
        OnCameraCenter(new InputAction.CallbackContext());
    }

    private void Disable(bool win,float _)
    {
        if(win) return;

        gameEnded = true;
        GameInputManager.OnCameraPerformed -= OnCamMovementPerformed;
        GameInputManager.OnCameraCancelled -= OnCamMovementCancelled;
        GameInputManager.OnCameraCenter -= OnCameraCenter;
    }

    private void OnCameraCenter(InputAction.CallbackContext obj)
    {
        doCenter = true;
        instantTCamPos = _currentCameraRot;
    }


    private void OnCamMovementPerformed(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
        if (invertXAxis) moveVector.x *= -1;
        if (invertYAxis) moveVector.y *= -1;
    }
    private void OnCamMovementCancelled(InputAction.CallbackContext ctx)
    {
        moveVector = Vector2.zero;

    }

    void Update()
    {
        if(gameEnded) return;
        
        transform.SetPositionAndRotation(spaceShip.position,spaceShip.rotation);
        
        _currentCameraRot.y += moveVector.x * MouseSensitivity*Time.deltaTime;
        // _currentCameraRot.y %= 360;
        _currentCameraRot.x += moveVector.y * MouseSensitivity*Time.deltaTime;
        //_currentCameraRot.x %= 360;

        if (doCenter)
        {
            if (timeElapsed < animDuration)
            {
                _currentCameraRot = Vector3.Lerp(instantTCamPos, Vector3.zero, timeElapsed / animDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                timeElapsed = 0f;
                doCenter = false;
                _currentCameraRot = Vector3.zero;
            }
        }
        
        

        child.localRotation =Quaternion.Euler(_currentCameraRot);
        //transform.Rotate(_currentCameraRot);

        
        //print(_currentCameraRot);
        //var rot = _currentCameraRot;
        
        //transform.rotation = Quaternion.Euler(_currentCameraRot);


    }
}
