using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField] public Transform SpawnPoint;

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
                Debug.Log("Fire 1");
                lastBullet = BulletPoolManager.instance.getBullet().gameObject;
                Debug.Log("Fire 2");
                break;
            case WeaponType.Rocket:
                Debug.Log("Rocket");
                lastBullet = BulletPoolManager.instance.getRocket().gameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log("Fire 3");
        lastBullet.transform.position = SpawnPoint.position;
        Debug.Log("Fire 4");
        lastBullet.transform.rotation = transform.rotation;
        Debug.Log("Fire 5");
        lastBullet.GetComponent<BulletParent>().SetData(data.lifeTime, data.speed, data.damage);
        Debug.Log("Fire 6");
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}