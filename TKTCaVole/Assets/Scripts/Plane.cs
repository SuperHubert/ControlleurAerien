using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour, IDamageable
{
    public static event Action OnPlaneDestroyed;

    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private string audioGameOver;
    [SerializeField] private int HP = 100;
    
    public static Weapon Rocket { get; private set; }
    

    private void Awake()
    {
        Rocket = secondaryWeapon;
    }

    private void Start()
    {
        primaryWeapon.SetSpawnPoint(SpawnPoint);
        secondaryWeapon.SetSpawnPoint(SpawnPoint);
        
        GameInputManager.OnPrimaryShootPerformed += PrimaryShootWeapon;
        GameInputManager.OnSecondaryShootPerformed += SecondaryShootWeapon;
        
        GameInputManager.OnPrimaryShootCancelled += StopPrimaryShootWeapon;
        GameInputManager.OnSecondaryShootCancelled += StopSecondaryShootWeapon;
    }

    private void PrimaryShootWeapon(InputAction.CallbackContext ctx)
    {
        if (gameObject.activeSelf && Time.timeScale > 0.1f)
            primaryWeapon.StartShoot();
    }
    
    private void StopPrimaryShootWeapon(InputAction.CallbackContext ctx)
    {
        if (gameObject.activeSelf)
            primaryWeapon.StopShoot();
    }

    private void SecondaryShootWeapon(InputAction.CallbackContext ctx)
    {
        if (gameObject.activeSelf && Time.timeScale > 0.1f)
            secondaryWeapon.StartShoot();
    }
    
    private void StopSecondaryShootWeapon(InputAction.CallbackContext ctx)
    {
        if (gameObject.activeSelf)
            secondaryWeapon.StopShoot();
    }

    public void TakeDamage(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            OnPlaneDestroyed?.Invoke();
            AudioManager.Instance.PlaySound(audioGameOver);
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