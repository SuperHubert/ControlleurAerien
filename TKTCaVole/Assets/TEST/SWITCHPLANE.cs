using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWITCHPLANE : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Plane plane;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            plane.weapon = _weapons[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            plane.weapon = _weapons[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            plane.weapon = _weapons[2];
    }
}
