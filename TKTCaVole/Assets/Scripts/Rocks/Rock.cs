using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rock : MonoBehaviour, IDamageable
{
    public int damage {get; private set;} = 9999;
    
    [SerializeField] private float explosionPower = 100f;
    [SerializeField] private int nbDebrisByRegularDamage = 5;
    [SerializeField] private Rigidbody _rigidbody;
    
    private int health;
    private bool DropsHourglass;
    private int healthratio = 200;
    private List<GameObject> debris = new();
    private float ratioStone = 0.5f;

    private void Start()
    {
        _rigidbody.angularVelocity = Random.onUnitSphere;
    }

    public void SetRockData(float size, bool drops, int hp)
    {
        transform.localScale = size * Vector3.one;
        DropsHourglass = drops;
        healthratio = hp;
        health = (int)size * healthratio;
        ratioStone = transform.GetChild(0).localScale.x;
    }

    public void TakeDamage(int Damage)
    {
        health -= Damage;
        float reelDamage = Damage / (float)healthratio;
        transform.localScale -= reelDamage * Vector3.one;

        int nbDebrisToSpawn = (1 + (int)reelDamage) * nbDebrisByRegularDamage;
        if (DebrisPoolManager.instance)
        {
            for (int i = 0; i < nbDebrisToSpawn; i++)
            {
                GameObject newDebris = DebrisPoolManager.instance.GetRandomDebris();
                debris.Add(newDebris);
                newDebris.transform.position = transform.position + Random.onUnitSphere * (transform.localScale.x + 1.5f * ratioStone);
                Rigidbody rb = newDebris.GetComponent<Rigidbody>();
                rb.velocity = (newDebris.transform.position - transform.position).normalized * explosionPower;
                rb.angularVelocity = Random.onUnitSphere;
            }

            if (health <= 0)
            {
                // foreach (GameObject obj in debris) // depop all debris
                // {
                //     DebrisPoolManager.instance.AddToPool(obj);
                // }
                if (DropsHourglass)
                {
                    GameObject hourglass = DebrisPoolManager.instance.GetHourglass();
                    hourglass.transform.position = transform.position;
                }

                Destroy(gameObject);
            }
        }
    }
}