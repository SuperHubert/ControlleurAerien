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
    [SerializeField] private int iterations;
    [SerializeField] private float distanceBetweenVectors = 500f;
    [SerializeField] private float maxDistanceFromCenter = 10000f;
    private Vector3 vectorForward => distanceBetweenVectors * Vector3.forward;
    [SerializeField] private float minAngle;
    [SerializeField] private LayerMask generationLayer;
    
    private void Start()
    {
        Random.InitState(seed);
        
        var rot = Quaternion.identity;
        var vector = distanceBetweenVectors*Vector3.forward;
        var previousPos = Vector3.zero;

        StartCoroutine(PlaceRingRoutine());
        
        IEnumerator PlaceRingRoutine()
        {
            for (int i = 0; i < iterations; i++)
            {
                rot = Random.rotation; //Should be based on previous rot
                vector += rot * vectorForward;

                Debug.DrawLine(previousPos, vector, Color.yellow, 100);

                var look = (vector - previousPos).normalized;

                if (!Physics.Raycast(previousPos, look, out var hit, distanceBetweenVectors, generationLayer))
                {
                    Debug.Log("No Hit");
                    
                    var ring = Instantiate(ringPrefab, (vector+previousPos)/2f, Quaternion.identity,ringParent);
                    ring.SetNumber(i);
                    ring.gameObject.name = $"Ring {i}";
                    ring.transform.forward = look;
            
                    previousPos = vector;
                }
                else
                {
                    Debug.Log("Hit !!!");
                }
                
                yield return null;
            }
        }
    }
    
}
