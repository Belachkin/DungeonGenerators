using Assets._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int _corridorLength = 14;
    [SerializeField] private int _corridorCount = 5;

    [Range(0.1f, 1)]
    [SerializeField] private float _roomPercent = 0.8f;

    [SerializeField] private bool _isInscreaseRooms = false;
    
    protected override void RunProcedualGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);


        List<Vector2Int> deadEnds = FindAdllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        if(_isInscreaseRooms)
        {
            for (int i = 0; i < corridors.Count; i++)
            {
                corridors[i] = InscreaseCorridorSizeByOne(corridors[i]);
                floorPositions.UnionWith(corridors[i]);
            }
        }

        _tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, _tilemapVisualizer);
    }

    private List<Vector2Int> InscreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if(previousDirection != Vector2Int.zero && directionFromCell != previousDirection)
            {
                for(int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1 ] + newCorridorTileOffset);
            }
        }

        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right) 
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;

        return Vector2Int.zero;
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloor)
    {
        foreach (var position in deadEnds)
        {
            if(!roomFloor.Contains(position))
            {
                var room = RunRandomWalk(_randomWalkParameters, position);
                roomFloor.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAdllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if(floorPositions.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }
            if(neighboursCount == 1)
            {
                deadEnds.Add(position);
            }
        }


        return deadEnds;
    }


    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPositions = _startPosition;
        potentialRoomPositions.Add(currentPositions);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < _corridorCount; i++)
        {
            var corridor = WalkDungeunGenerator.RandomWalkCorridor(currentPositions, _corridorLength);

            corridors.Add(corridor);

            currentPositions = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPositions);
            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }
    
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * _roomPercent);

        //Guid.NewGuid() - ���������� ������������� �� �������� �� ����� ����������� (https://guid.one/guid)
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(_randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }
}
