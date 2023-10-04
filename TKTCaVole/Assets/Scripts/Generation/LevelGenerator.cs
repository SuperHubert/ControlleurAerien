using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Gate ringPrefab;
    [SerializeField] private List<Rock> rockPrefabs;

    [Header("Parents")]
    [SerializeField] private Transform ringParent;

    [Header("Settings")]
    [SerializeField] private float distanceBetweenVectors = 500f;
    [SerializeField] private float distanceMultiplier = 3f;
    private float maxDistanceFromCenter => distanceBetweenVectors * distanceMultiplier;

    [Header("Rocks")]
    [SerializeField] private Vector2Int rockCountRange;
    [SerializeField] private Vector2 rockDistanceRange;
    [SerializeField] private Vector2 rockScale;
    [SerializeField] private int hpRatio = 200;
    
    private Vector3 VectorForward => distanceBetweenVectors * Vector3.forward;
    [SerializeField] private LayerMask generationLayer;
    
    
    public void GenerateLevel(int seed,Action callback)
    {
        Random.InitState(seed);

        var iterations = seed + 6;
        
        var worldCenter = Vector3.zero;
        var rot = Quaternion.identity;
        var vector = distanceBetweenVectors*Vector3.forward;
        var previousPos = Vector3.zero;
        var look = (vector - previousPos).normalized;

        StartCoroutine(PlaceRingRoutine());
        
        IEnumerator PlaceRingRoutine()
        {
            for (int i = 0; i < iterations; i++)
            {
                var isFirstLevel = seed == 0;
                var noMaxDistance = seed <= 1;
                
                var x = Random.Range(-90f, 90f);
                var y = Random.Range(-90f, 90f);
                var z = isFirstLevel ? 0 : Random.Range(-90f, 90f);

                rot = Quaternion.AngleAxis(x, Vector3.forward);
                vector += noMaxDistance ? rot * VectorForward : (Vector3.Distance(worldCenter, vector + (rot * VectorForward)) > maxDistanceFromCenter) ? rot * -VectorForward : rot * VectorForward;
                
                rot = Quaternion.AngleAxis(y, Vector3.up);
                vector += noMaxDistance ? rot * VectorForward : (Vector3.Distance(worldCenter, vector + (rot * VectorForward)) > maxDistanceFromCenter) ? rot * -VectorForward : rot * VectorForward;
                
                rot = Quaternion.AngleAxis(z, Vector3.right);
                vector += noMaxDistance ? rot * VectorForward : (Vector3.Distance(worldCenter, vector + (rot * VectorForward)) > maxDistanceFromCenter) ? rot * -VectorForward : rot * VectorForward;
                
                Debug.DrawLine(previousPos, vector, Color.yellow, 100);

                look = (vector - previousPos).normalized;

                if (!Physics.Raycast(previousPos, look, out var hit, distanceBetweenVectors, generationLayer))
                {
                    //  Debug.Log("No Hit");

                    var pos = (vector + previousPos) / 2f;
                    var ring = Instantiate(ringPrefab, pos, Quaternion.identity,ringParent);
                    ring.gameObject.name = $"Gate {i}";
                    ring.transform.forward = look;

                    var rockCount = Random.Range(rockCountRange.x, rockCountRange.y);
                    
                    for (int j = 0; j < rockCount; j++)
                    {
                        if (!PlaceRock(pos)) j--;
                        yield return null;
                    }
            
                    previousPos = vector;
                }
                else
                {
                    Debug.Log("Hit !!!");
                    i--;
                }
                
                Gate.InitGates(iterations);
                
                yield return null;
                
                bool PlaceRock(Vector3 origin)
                {
                    var rockPos = Random.onUnitSphere * Random.Range(rockDistanceRange.x, rockDistanceRange.y) + origin;

                    if (Physics.Raycast(origin,(rockPos-origin).normalized, out var rockHit,Vector3.Distance(origin,rockPos), generationLayer)) return false;

                    var rock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Count)], rockPos, Random.rotation,
                        ringParent);
                    
                    rock.SetRockData(Random.Range(rockScale.x, rockScale.y),true,hpRatio);
                    
                    rock.name = "Rock";
                    
                    return true;
                }
            }
            
            callback.Invoke();
        }
    }
}
