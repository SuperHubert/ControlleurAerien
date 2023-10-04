using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform spaceShip;
    private Vector2 moveVector = Vector2.zero;

    private Vector3 _currentCameraRot = new Vector3(0, 0, -0);
    
    public float MouseSensitivity;

    private bool doCenter = false;
    private float animDuration = 0.1f;
    private float timeElapsed = .0f;
    private Vector3 instantTCamPos;
    
    private void Start()
    {
        GameInputManager.OnCameraPerformed += OnCamMovementPerformed;
        GameInputManager.OnCameraCancelled += OnCamMovementCancelled;
        GameInputManager.OnCameraCenter += OnCameraCenter;
    }

    private void OnCameraCenter(InputAction.CallbackContext obj)
    {
        doCenter = true;
        instantTCamPos = _currentCameraRot;
    }


    private void OnCamMovementPerformed(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
    }
    private void OnCamMovementCancelled(InputAction.CallbackContext ctx)
    {
        moveVector = Vector2.zero;

    }

    void Update()
    {
        transform.rotation = spaceShip.rotation;
        
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
        
        transform.Rotate(_currentCameraRot);

        
        //print(_currentCameraRot);
        //var rot = _currentCameraRot;
        
        //transform.rotation = Quaternion.Euler(_currentCameraRot);


    }
}
