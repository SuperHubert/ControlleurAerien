using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParent : MonoBehaviour
{
    [SerializeField] protected float lifeTime;
    [SerializeField] protected Rigidbody rg;
    [SerializeField] protected float speed;
    
    protected IEnumerator FinalCountDown()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
