using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float size = 1f;
        float yOffset = Mathf.Sqrt(3) * size / 2;
        float xOffset = size * 1.5f;

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
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (y % 2 == 0);
                spawnedTile.Init(isOffset);
            }
        }
        _cam.transform.position = new Vector3((float)_width/2, (float)_height / 2 - 0.5f, -10);
    }

    void Update()
    {
        
    }
}
