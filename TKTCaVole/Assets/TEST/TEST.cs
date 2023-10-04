using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    [SerializeField] private List<Rock> rocks;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var rock in rocks)
        {
            rock.SetRockData(3.5f,true,200);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
