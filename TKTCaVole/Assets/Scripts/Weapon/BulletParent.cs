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

    public void SetData(float _lifeTime, float _speed)
    {
        lifeTime = _lifeTime;
        speed = _speed;
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
        if (!other.GetComponent<Enemy>())
            Destroy(other.gameObject);
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