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
    private bool wantFire = false;

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
        if (wantFire)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(Shootswsssssszs());
        }
    }

    private IEnumerator Shootswsssssszs()
    {
        lastBullet = null;
        if (canFire)
        {
            StartCoroutine(reload());
            if (!BulletPoolManager.instance) yield break;
            switch (data.type)
            {
                case WeaponType.PewPew:
                    lastBullet = BulletPoolManager.instance.getBullet(SpawnPoint.position, transform.rotation)
                        .gameObject;
                    break;
                case WeaponType.Rocket:
                    lastBullet = BulletPoolManager.instance.getRocket(SpawnPoint.position, transform.rotation)
                        .gameObject;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lastBullet.GetComponent<BulletParent>().SetData(data.lifeTime, data.speed, data.damage);
        }
    }

    public virtual void StartShoot()
    {
        wantFire = true;
        StartCoroutine(Shootswsssssszs());
    }

    public virtual void StopShoot()
    {
        wantFire = false;
    }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}