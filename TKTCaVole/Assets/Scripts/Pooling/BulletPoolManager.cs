using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    [SerializeField] private int nbBullet = 20;
    [SerializeField] private int nbRocket = 10;
    [SerializeField] private WeaponData bulletData;
    [SerializeField] private WeaponData rocketData;

    private List<Bullet> bulletPool = new ();
    private List<Rocket> rocketPool = new ();

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < nbBullet; i++)
        {
            GameObject obj = Instantiate(bulletData.BulletPrefab, transform);
            obj.SetActive(false);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetData(bulletData.lifeTime, bulletData.speed, bulletData.damage);
            bulletPool.Add(bullet);
        }

        for (int i = 0; i < nbRocket; i++)
        {
            GameObject obj = Instantiate(rocketData.BulletPrefab, transform);
            obj.SetActive(false);
            Rocket rocket = obj.GetComponent<Rocket>();
            
            rocket.SetData(bulletData.lifeTime, bulletData.speed, bulletData.damage);
            rocketPool.Add(rocket);
        }
    }

    public void AddToPool<T>(T bullet) where T : BulletParent
    {
        bullet.gameObject.SetActive(false);
        switch (bullet)
        {
            case Bullet myBullet:
                bulletPool.Add(myBullet);
                break;
            case Rocket rocket:
                rocketPool.Add(rocket);
                break;
        }
    }

    public Rocket getRocket(Vector3 pos)
    {
        Rocket obj = null;
        if (rocketPool.Count > 0)
        {
            obj = rocketPool[0];
            rocketPool.RemoveAt(0);
        }
        else
            obj = Instantiate(rocketData.BulletPrefab, transform).GetComponent<Rocket>();
        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        return obj;
    }
    
    public Bullet getBullet(Vector3 pos)
    {
        Bullet obj = null;
        if (bulletPool.Count > 0)
        {
            obj = bulletPool[0];
            bulletPool.RemoveAt(0);
        }
        else
            obj = Instantiate(bulletData.BulletPrefab, transform).GetComponent<Bullet>();

        obj.transform.position = pos;
        obj.gameObject.SetActive(true);
        return obj;
    }
}