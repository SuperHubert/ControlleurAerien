using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Gate ringPrefab;
    [SerializeField] private List<GameObject> rockPrefabs;
    [SerializeField] private GameObject shipPrefab;

    [Header("Parents")]
    [SerializeField] private Transform ringParent;

    [Header("Settings")]
    [SerializeField] private int seed;
    private int realSeed;
    [SerializeField] private int iterations;
    [SerializeField] private float distanceBetweenVectors = 500f;
    [SerializeField] private float maxDistanceFromCenter = 10000f;

    [Header("Rocks")]
    [SerializeField] private Vector2Int rockCountRange;
    [SerializeField] private Vector2 rockDistanceRange;
    
    private Vector3 vectorForward => distanceBetweenVectors * Vector3.forward;
    [SerializeField] private float minAngle;
    [SerializeField] private LayerMask generationLayer;
    
    private void Start()
    {
        realSeed = seed != 0 ? seed : Random.Range(99999,999999);
        Random.InitState(realSeed);

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
                var x = Random.Range(-90f, 90f);
                var y = Random.Range(-90f, 90f);
                var z = Random.Range(-90f, 90f);

                rot = Quaternion.AngleAxis(x, Vector3.forward);
                vector += (Vector3.Distance(worldCenter, vector + (rot * vectorForward)) > maxDistanceFromCenter) ? rot * -vectorForward : rot * vectorForward;
                
                rot = Quaternion.AngleAxis(y, Vector3.up);
                vector += (Vector3.Distance(worldCenter, vector + (rot * vectorForward)) > maxDistanceFromCenter) ? rot * -vectorForward : rot * vectorForward;
                
                rot = Quaternion.AngleAxis(z, Vector3.right);
                vector += (Vector3.Distance(worldCenter, vector + (rot * vectorForward)) > maxDistanceFromCenter) ? rot * -vectorForward : rot * vectorForward;
                
                Debug.DrawLine(previousPos, vector, Color.yellow, 100);

                look = (vector - previousPos).normalized;

                if (!Physics.Raycast(previousPos, look, out var hit, distanceBetweenVectors, generationLayer))
                {
                    Debug.Log("No Hit");

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
                }
                
                yield return null;

                bool PlaceRock(Vector3 origin)
                {
                    var rockPos = Random.onUnitSphere * Random.Range(rockDistanceRange.x, rockDistanceRange.y) + origin;

                    if (Physics.Raycast(origin,(rockPos-origin).normalized, out var rockHit,Vector3.Distance(origin,rockPos), generationLayer)) return false;

                    var rock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Count)], rockPos, Random.rotation,
                        ringParent);

                    rock.name = "Rock";
                    
                    return true;
                }
            }
        }
    }
    
}
