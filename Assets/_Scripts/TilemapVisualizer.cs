using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap _floorTilemap;
    [SerializeField] private TileBase _floorTile;
    

    //[SerializeField] private Tilemap _wallTilemap;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, _floorTilemap, _floorTile);
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(position, tilemap, tile);
        }
    }

    public void PaintSingleTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);

        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        _floorTilemap.ClearAllTiles();
    }
}
