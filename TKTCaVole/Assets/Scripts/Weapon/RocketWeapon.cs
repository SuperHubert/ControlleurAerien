using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketWeapon : Weapon
{
    float isInFrontDistance(Transform target){
        return Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(target.position));
    }
    
    
    
    public override void Shoot()
    {
        base.Shoot();
        if (!lastBullet) return;
        Rocket rocket = lastBullet.GetComponent<Rocket>();
        if (!rocket) return;
        
        
        Transform target = null; // a supprimer
        List<Transform> allEnemies = new List<Transform>();

        Enemy Myenemy = FindObjectOfType<Enemy>(); // a changer
        if (Myenemy)
        allEnemies.Add(Myenemy.transform); // a changer
        
        
        float distance = 0;
        foreach (var enemy in allEnemies)
        {
            float distancePotential = isInFrontDistance(enemy);
            
            if (!(distancePotential >= 10f)) continue;// center cam 
            if (distancePotential > distance)
                distance = distancePotential;

            target = enemy;
        }
        if (!target) return;
        
        rocket.AssignTarget(target);
    }
}