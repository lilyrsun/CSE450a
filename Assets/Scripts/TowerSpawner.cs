using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public GameObject towerPrefab;
    public int tileX = 2;
    public int tileY = 3;

    void Start()
    {
        // Tile name that the script is looking for
        string tileName = $"Tile {tileX} {tileY}";
        Debug.Log("Looking for tile: " + tileName);

        // Find all Tile objects in the scene
        Tile[] allTiles = FindObjectsOfType<Tile>();

        Debug.Log("Total tiles found: " + allTiles.Length);  // Should be >0 if tiles exist

        GameObject targetTile = null;

        foreach (Tile tile in allTiles)
        {
            Debug.Log("Checking tile: " + tile.gameObject.name);
            if (tile.gameObject.name == tileName)
            {
                targetTile = tile.gameObject;
                Debug.Log("Found matching tile!");
                break;
            }
        }

        if (targetTile != null)
        {
            // Adjust spawn position to make sure it's visible
            Vector3 spawnPosition = targetTile.transform.position; // Lift & adjust z-position

            // Spawn tower with increased scale
            GameObject tower = Instantiate(towerPrefab, spawnPosition, Quaternion.identity);
            //tower.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Make sure it's visible

            // Corrected Debug.Log Statement
            Debug.Log("Tower spawned on " + tileName);
        }
    }
} // <-- Closing brace for TowerSpawner class
