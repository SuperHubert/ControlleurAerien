using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform spaceShip;
    private Vector3 _currentCameraRot = new Vector3(0, 0, -0);
    public int mouseSensitivity;

    // Update is called once per frame
    void Update()
    {
        transform.position = spaceShip.position;
        
        float mouseXMov = Input.GetAxis("Mouse X");
        float mouseYMov = Input.GetAxis("Mouse Y");

        _currentCameraRot.y += mouseXMov*mouseSensitivity;
        _currentCameraRot.x += mouseYMov*mouseSensitivity;

        transform.rotation = Quaternion.Euler(_currentCameraRot);



    }
}
