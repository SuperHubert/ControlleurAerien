using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData data;
    [SerializeField] public Transform SpawnPoint;
    
    protected BulletParent lastBullet;
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
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        lastBullet = null;
        if (!canFire) yield break;

        StartCoroutine(reload());
        
        if(!BulletPoolManager.instance) yield break;

        lastBullet = data.type switch
        {
            WeaponType.PewPew => BulletPoolManager.instance.getBullet(SpawnPoint.position, transform.rotation),
            WeaponType.Rocket => BulletPoolManager.instance.getRocket(SpawnPoint.position, transform.rotation),
            _ => throw new ArgumentOutOfRangeException()
        };
        lastBullet.SetData(data.lifeTime, data.speed, data.damage);
        AudioManager.Instance.PlaySound(data.SoundKey);
    }

    public virtual void StartShoot()
    {
        wantFire = true;
        StartCoroutine(Shoot());
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