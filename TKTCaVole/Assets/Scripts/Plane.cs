using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour, IDamageable
{
    public static event Action OnPlaneDestroyed;
    
    [SerializeField] public Weapon primaryWeapon;
    [SerializeField] public Weapon secondaryWeapon;
    [SerializeField] public Transform SpawnPoint;
    [SerializeField] private int HP = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        primaryWeapon.SetSpawnPoint(SpawnPoint);
        secondaryWeapon.SetSpawnPoint(SpawnPoint);
        GameInputManager.OnPrimaryShootPerformed += PrimaryShootWeapon;
        GameInputManager.OnSecondaryShootPerformed += SecondaryShootWeapon;
    }

    private void PrimaryShootWeapon(InputAction.CallbackContext ctx)
    {
        primaryWeapon.Shoot();
    }
    
    private void SecondaryShootWeapon(InputAction.CallbackContext ctx)
    {
        secondaryWeapon.Shoot();
    }

    public void TakeDamage(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            OnPlaneDestroyed?.Invoke();
           gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Rock rock = other.gameObject.GetComponent<Rock>();
        if (rock)
        {
            TakeDamage(rock.damage);
        }
    }
}
