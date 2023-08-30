using Assets._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int _minRoomWidth = 4;
    [SerializeField] private int _minRoomHeight = 4;

    [SerializeField] private int _dungeonWidth = 20;
    [SerializeField] private int _dungeonHeight = 20;

    [Range(0, 10)]
    [SerializeField] private int _offset = 1;
    [SerializeField] private bool _randomWalkRooms = false;

    protected override void RunProcedualGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = WalkDungeunGenerator.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)_startPosition, 
            new Vector3Int(_dungeonWidth, _dungeonHeight, 0)), 
            _minRoomWidth, _minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        floor = CreateSimpleRooms(roomList);

        //List<Vector2Int> roomCenters = new List<Vector2Int>();

        //foreach (var room in roomList)
        //{
        //    roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        //}

        //HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);

        _tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, _tilemapVisualizer);

        

    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = _offset; col < room.size.x - _offset; col++)
            {
                for (int row = _offset; row < room.size.y - _offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}
