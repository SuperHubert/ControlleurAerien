using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;

    protected GameObject lastBullet;

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
        lastBullet = null;
         if (!canFire) return;
        StartCoroutine(reload());
        lastBullet = Instantiate(data.BulletPrefab, transform.forward + transform.position, transform.rotation);
        lastBullet.GetComponent<BulletParent>().SetData(data.lifeTime, data.speed);
    }
}