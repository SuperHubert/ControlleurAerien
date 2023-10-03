using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] protected Rigidbody rg;
    [SerializeField] protected BoxCollider coll;
    protected float lifeTime;
    protected float speed;
    protected int damage;

    protected virtual void OnEnable()
    {
        StartCoroutine(FinalCountDown());
    }

    public void SetData(float _lifeTime, float _speed, int _damage)
    {
        lifeTime = _lifeTime;
        speed = _speed;
        damage = _damage;
    }

    protected IEnumerator FinalCountDown()
    {
        yield return new WaitForSeconds(lifeTime);
        switch (this)
        {
            case Bullet:
                BulletPoolManager.instance.AddToPool(this as Bullet);
                break;
            case Rocket:
                BulletPoolManager.instance.AddToPool(this as Rocket);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
            target.TakeDamage(damage);
        switch (this)
        {
            case Bullet:
                BulletPoolManager.instance.AddToPool(this as Bullet);
                break;
            case Rocket:
                BulletPoolManager.instance.AddToPool(this as Rocket);
                break;
        }
    }
}