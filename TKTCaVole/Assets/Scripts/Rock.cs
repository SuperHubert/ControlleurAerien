using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IDamageable
{
    public int damage {get; private set;} = 9999;
    
    private int health;
    private bool DropsHourglass;
    private int healthratio = 200;
    private List<GameObject> debris = new();
    private float ratioStone = 0.5f;

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
        float reelDamage = Damage / healthratio;
        transform.localScale -= reelDamage * Vector3.one;

        //spawn particles here
        if (DebrisPoolManager.instance)
        {
            for (int i = 0; i < reelDamage; i++)
            {
                GameObject newDebris = DebrisPoolManager.instance.GetRandomDebris();
                debris.Add(newDebris);
                newDebris.transform.position = transform.position + Random.onUnitSphere * (transform.localScale.x + reelDamage + 1.5f * ratioStone);
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