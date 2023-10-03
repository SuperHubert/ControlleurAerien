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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Enemy>())
            Destroy(other.gameObject);
        Destroy(gameObject);
    }
}