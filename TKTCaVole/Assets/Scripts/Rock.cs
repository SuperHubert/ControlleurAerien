using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IDamageable
{
    public bool DropsHourglass { get; private set; }
    private int health;

    private int healthratio = 200;
    private List<GameObject> debris = new();

    public void SetRockData(float size, bool drops, int hp)
    {
        transform.localScale = size * Vector3.one;
        DropsHourglass = drops;
        healthratio = hp;
        health = (int)size * healthratio;
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
                newDebris.transform.position = transform.position + Random.insideUnitSphere * 5f;
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