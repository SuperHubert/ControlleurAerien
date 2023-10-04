using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IDamageable
{
    public bool DropsHourglass { get; private set; }
    private int health;

    public bool SetRockData(bool drops,int hp)
    {
        DropsHourglass = drops;
        health = hp;
    }
    
    public void TakeDamage(int Damage)
    {
        health -= Damage;
    }
}
