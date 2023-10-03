using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public float speed = 100.0F;
    public int gear = 0;
    public int gearValue = 20;
    public float rotationSpeed = 75.0F;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = speed + gear*gearValue;
        
        float rotationSpeedVertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        float rotationSpeedHorizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        
        transform.Translate(0, 0, currentSpeed * Time.deltaTime);
        transform.Rotate(rotationSpeedVertical, rotationSpeedHorizontal, 0);

        if (Input.GetKey(KeyCode.E) && gear < 5) gear++; //changer le FOV
        if (Input.GetKey(KeyCode.A) && gear > 0) gear--;


    }

}
