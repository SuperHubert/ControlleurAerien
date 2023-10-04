using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWITCHPLANE : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Plane plane;

    private void Start()
    {
        //plane.weapon = _weapons[0];
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     plane.weapon = _weapons[0];
        //     plane.weapon.SetSpawnPoint(plane.SpawnPoint);
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     plane.weapon = _weapons[1];
        //     plane.weapon.SetSpawnPoint(plane.SpawnPoint);
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     plane.weapon = _weapons[2];
        //     plane.weapon.SetSpawnPoint(plane.SpawnPoint);
        // }
        //transform.position += Vector3.forward * Time.deltaTime * 10f;
    }
}