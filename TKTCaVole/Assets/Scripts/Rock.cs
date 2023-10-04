using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IDamageable
{
    public bool DropsHourglass { get; private set; }
    private int health;

    public void SetRockData(float size,bool drops,int hp)
    {
        transform.localScale = size * Vector3.one;
        DropsHourglass = drops;
        health = hp;
    }
    
    public void TakeDamage(int Damage)
    {
        health -= Damage;
    }
}
