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

        Debug.Log("Fire !");
        switch (data.type)
        {
            case WeaponType.PewPew:
                lastBullet = BulletPoolManager.instance.getBullet().gameObject;
                break;
            case WeaponType.Rocket:
                Debug.Log("Rocket");
                lastBullet = BulletPoolManager.instance.getRocket().gameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        lastBullet.transform.position = transform.position + transform.forward;
        lastBullet.transform.rotation = transform.rotation;
        lastBullet.GetComponent<BulletParent>().SetData(data.lifeTime, data.speed, data.damage);
    }
}