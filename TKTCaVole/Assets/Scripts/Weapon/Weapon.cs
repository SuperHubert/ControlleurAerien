using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField] public Transform SpawnPoint;

    protected GameObject lastBullet;
    public event Action<float> OnReloadStart; 
    protected bool canFire = true;

    private void Start()
    {
        canFire = true;
    }

    private IEnumerator reload()
    {
        canFire = false;
        OnReloadStart?.Invoke(data.cooldown);
        yield return new WaitForSeconds(data.cooldown);
        canFire = true;
    }

    public virtual void Shoot()
    {
        lastBullet = null;
        if (!canFire) return;
        StartCoroutine(reload());

        if (!BulletPoolManager.instance) return;
        switch (data.type)
        {
            case WeaponType.PewPew:
                lastBullet = BulletPoolManager.instance.getBullet(SpawnPoint.position, transform.rotation).gameObject;
                break;
            case WeaponType.Rocket:
                lastBullet = BulletPoolManager.instance.getRocket(SpawnPoint.position, transform.rotation).gameObject;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        lastBullet.GetComponent<BulletParent>().SetData(data.lifeTime, data.speed, data.damage);
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}