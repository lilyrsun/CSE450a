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
    [SerializeField] private Tile _grassTile, _mountainTile;
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

        for (int x = 0; x < _width-2; x++)
        {
            for (int y = 0; y < _height+10; y++)
            {
                float yPos = (y * yOffset)/2;
                float xPos = x * xOffset-2;
                if (y % 2 == 1)
                {
                    xPos += xOffset / 2;
                }
                Tile tileToSpawn = /*(x == 5 && y == 15 || x == 5 && y == 13 || x == 6 && y == 14 || x == 6 && y == 12 || x == 7 && y ==4 || x == 7 && y == 6 || x == 7 && y == 5 || x == 7 && y == 3) ? _mountainTile :*/ _grassTile;

                var spawnedTile = Instantiate(tileToSpawn, new Vector3(xPos-7.64f, yPos-4.21f, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x,y);
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        //_cam.transform.position = new Vector3((float)_width/2, (float)_height / 2 - 0.5f, -10);

        gameManager.Instance.ChangeState(GameState.SpawnUnit);
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


    //public Tile GetUnitSpawnTile()
    //{
    //    return _tiles.Where(t => t.Key.x < _width / 3 && t.Value.walkable && !(t.Key.x == 3 && t.Key.y == 10)).OrderBy(t => Random.value).First().Value;
    //}

    public Tile GetNPCUnitSpawnTile()
    {
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.walkable && !(t.Key.x == 10 && t.Key.y == 13)).OrderBy(t => Random.value).First().Value;
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
}
