using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    // Assign tower prefab
    public GameObject towerPrefab;

    // X coordinate of the tile to spawn on
    public int tileX = 2;

    // Y coordinate of the tile to spawn on
    public int tileY = 3;

    void Start()
    {
        // tile name using the same format from GridManager
        string tileName = $"Tile {tileX} {tileY}";

        GameObject targetTile = GameObject.Find(tileName);

        if (targetTile != null)
        {
            Instantiate(towerPrefab, targetTile.transform.position, Quaternion.identity);
            Debug.Log("Tower spawned on " + tileName);
        }
        else
        {
            Debug.LogError("Tile not found: " + tileName);
        }

    }
}
