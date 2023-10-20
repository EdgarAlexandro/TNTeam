using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterGrid : MonoBehaviour
{
    public Tilemap water;
    void Start()
    {
        water = GetComponent<Tilemap>();
    }

   
    private void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3Int cellPosition = water.WorldToCell(other.transform.position);
        }
    }
}
