using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform spaceShip;
    private Vector2 moveVector = Vector2.zero;

    private Vector3 _currentCameraRot = new Vector3(0, 0, -0);
    [Range(0.1f,1f)]
    public float MouseSensitivity;
    private void Start()
    {
        GameInputManager.OnCameraPerformed += OnCamMovementPerformed;
        GameInputManager.OnCameraCancelled += OnCamMovementCancelled;
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
        
        _currentCameraRot.y += moveVector.x * MouseSensitivity;
        // _currentCameraRot.y %= 360;
        _currentCameraRot.x += moveVector.y * MouseSensitivity;
        //_currentCameraRot.x %= 360;
        
        transform.Rotate(_currentCameraRot);

        
        //print(_currentCameraRot);
        //var rot = _currentCameraRot;
        
        //transform.rotation = Quaternion.Euler(_currentCameraRot);


    }
}
