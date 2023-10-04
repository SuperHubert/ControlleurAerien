using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Transform SpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        weapon.SetSpawnPoint(SpawnPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            weapon.Shoot();
    }
}
