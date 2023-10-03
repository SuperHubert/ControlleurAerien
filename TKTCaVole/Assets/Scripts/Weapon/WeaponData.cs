using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PewPew,
    Rocket
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData", order = 0)]
public class WeaponData : ScriptableObject
{
    public GameObject BulletPrefab;
    public float cooldown;
    public float speed;
    public float lifeTime;
    public WeaponType type;
    public int damage;
}
