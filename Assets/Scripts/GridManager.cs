using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _grassTile, _mountainTile, _waterTile;
    [SerializeField] private Transform _cam;
    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
    } 

    public void GenerateGrid()
    {
        float size = 1f;
        float yOffset = Mathf.Sqrt(3) * size / 2;
        float xOffset = size * 1.5f;
        _tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < _width - 2; x++)
        {
            for (int y = 0; y < _height + 10; y++)
            {
                float yPos = (y * yOffset) / 2;
                float xPos = x * xOffset - 2;
                if (y % 2 == 1)
                {
                    xPos += xOffset / 2;
                }

                // Your complex conditions for water and mountain tiles.
                bool isWaterTile = ((x == 1 && y % 2 == 0 && y != 0 && y <= 16) 
                                    || (x == 1 && (y == 17 || y == 3)) 
                                    || (y == 1 || (x != 4 && x != 3 && y == 2) || y == 0) 
                                    || (x == 12 && y % 2 == 0) 
                                    || (y == 17 || y == 18) 
                                    || (x >= 4 && x <= 6 && (y == 15 || y == 16)) 
                                    || (x == 7 && y == 16)
                                    || ((x == 6) && (y >=7 && y<=10))  
                                    || (x == 5 && y == 9)
                                    || (x == 7 && y == 8)
                                    || (x == 11 && (y == 16 || y == 15)));
                bool isMountainTile = ((x == 0 && y == 0) 
                                       || (x == 0 && y >= 1 && y <= 17) 
                                       || x == 13 
                                       || (x == 12 && y % 2 != 0))|| (x==7 && y ==15) || (x==8 && y ==16) || (x==9 && y ==10) || (x==9 && y ==12) || (x==10 && y ==8) || (x == 10 && y == 8) || (x == 10 && y == 7) || (x == 10 && y == 8) || (x == 11 && y == 8);
                // Use a clear ternary chain.
                Tile tileToSpawn = isWaterTile ? _waterTile : isMountainTile ? _mountainTile : _grassTile;
                

                var spawnedTile = Instantiate(tileToSpawn, new Vector3(xPos - 7.64f, yPos - 4.21f, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);

                // Explicitly assign tile type.
                if (isWaterTile)
                    spawnedTile.tileType = Tile.TileType.Water;
                else if (isMountainTile)
                    spawnedTile.tileType = Tile.TileType.Mountain;
                else
                    spawnedTile.tileType = Tile.TileType.Grass;

                if ((x == 0 && y >= 0 && y <= 18) || (x ==1 && y == 0) || (x == 12 && y == 18) || (x ==12 && y % 2 != 0) || (x > 12) || (x == 1 && y ==18))
                {
                    spawnedTile.gameObject.SetActive(false);
                }

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        //_cam.transform.position = new Vector3((float)_width/2, (float)_height / 2 - 0.5f, -10);

        // Delay fog initialization until after grid is built.
        StartCoroutine(DelayedFogInit());
    }

    public Tile GetUnitSpawnTile()
    {
        int leftBoundary = 1;
        int rightBoundary = (_width / 3) - 1;
        int topBoundary = 1;
        int bottomBoundary = _height - 2;

        return _tiles
            .Where(t =>
                t.Key.x >= leftBoundary && t.Key.x <= rightBoundary &&
                t.Key.y >= topBoundary && t.Key.y <= bottomBoundary &&
                t.Value.walkable &&
                !(t.Key.x == 3 && t.Key.y == 10)
            )
            .OrderBy(t => Random.value)
            .FirstOrDefault().Value;
    }

    public Tile GetNPCUnitSpawnTile()
    {
        return _tiles
            .Where(t => t.Key.x > _width / 2 && t.Value.walkable && !(t.Key.x == 10 && t.Key.y == 13))
            .OrderBy(t => Random.value)
            .First().Value;
    }

    public Tile GetTileAtPosition(int x, int y)
    {
        Vector2 tileKey = new Vector2(x, y);
        if (_tiles.ContainsKey(tileKey))
        {
            return _tiles[tileKey];
        }
        return null;
    }

    void Update()
    {
        
    }

    private IEnumerator DelayedFogInit()
    {
        yield return new WaitForSeconds(0.1f); // give Unity a frame to catch up

        // Only initialize fog on grass tiles.
        foreach (var tile in FindObjectsOfType<Tile>())
        {   
            tile.isRevealed = false;
            tile.isVisible = false;
            tile.UpdateFog();
        }

        gameManager.Instance.RevealTilesAroundUnits();
        gameManager.Instance.ChangeState(GameState.SpawnUnit);
    }
}
