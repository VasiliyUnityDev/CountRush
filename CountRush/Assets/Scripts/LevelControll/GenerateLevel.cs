using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public List<GameObject> listTiles = new List<GameObject>();

    private void Awake()
    {
        Instantiate(listTiles[Random.Range(0, 9)], transform.position, Quaternion.identity);
    }
}
