using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform spaceShip;
    private Vector3 _currentCameraRot = new Vector3(0, 0, -0);

    // Update is called once per frame
    void Update()
    {
        transform.position = spaceShip.position;
        
        float mouseXMov = Input.GetAxis("Mouse X");
        float mouseYMov = Input.GetAxis("Mouse Y");
        //_currentCameraRot = transform.parent.rotation.eulerAngles;
        _currentCameraRot.y += mouseXMov;
        _currentCameraRot.x += mouseYMov;
        
        transform.rotation = Quaternion.Euler(_currentCameraRot);



    }
}
