using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lerpSpeedDisplacement;
    [SerializeField] private float lerpSpeedRotation;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeedDisplacement*Time.deltaTime);
        transform.rotation= Quaternion.Slerp(transform.rotation, target.rotation, lerpSpeedRotation*Time.deltaTime);
    }
}
