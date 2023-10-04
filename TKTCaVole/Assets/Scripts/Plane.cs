using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public Transform SpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        weapon.SetSpawnPoint(SpawnPoint);
        GameInputManager.OnShootPerformed += ShootWeapon;
    }

    private void ShootWeapon(InputAction.CallbackContext ctx)
    {
        weapon.Shoot();
    }
}
