using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;

    protected bool canFire = true;

    private void Start()
    {
        canFire = true;
    }

    private IEnumerator reload()
    {
        canFire = false;
        yield return new WaitForSeconds(data.cooldown);
        canFire = true;
    }

    public virtual void Shoot()
    {
        Debug.Log(canFire);
        if (!canFire) return;
        StartCoroutine(reload());
        Instantiate(data.BulletPrefab, transform.forward, transform.rotation, transform);
        
    }
}