using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] protected Rigidbody rg;
    [SerializeField] protected BoxCollider coll;
    protected float lifeTime;
    protected float speed;
    protected int damage;

    [SerializeField] private ParticleSystem particle;

    protected virtual void OnEnable()
    {
    }

    private void OnDisable()
    {
        
    }

    public void SetData(float _lifeTime, float _speed, int _damage)
    {
        lifeTime = _lifeTime;
        speed = _speed;
        damage = _damage;
        if (gameObject.activeSelf)
            StartCoroutine(FinalCountDown());
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
        target?.TakeDamage(damage);
        switch (this)
        {
            case Bullet:
                BulletPoolManager.instance.AddToPool(this as Bullet);
                break;
            case Rocket:
                BulletPoolManager.instance.AddToPool(this as Rocket);
                break;
        }
        print("ParticleSpawned");
        ParticleSystem particleObj = Instantiate(particle, transform.position, transform.rotation);
        //Destroy(particleObj,0.2f);
    }
}