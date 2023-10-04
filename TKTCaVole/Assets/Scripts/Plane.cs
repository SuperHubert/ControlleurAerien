using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour, IDamageable
{
    public static event Action OnPlaneDestroyed;
    
    [SerializeField] public Weapon weapon;
    [SerializeField] public Transform SpawnPoint;
    [SerializeField] private int HP = 100;
    
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

    public void TakeDamage(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            OnPlaneDestroyed?.Invoke();
            Destroy(gameObject);
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
