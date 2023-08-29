using Assets._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int _corridorLength = 14;
    [SerializeField] private int _corridorCount = 5;

    [Range(0.1f, 1)]
    [SerializeField] private float _roomPercent = 0.8f;
    public SimpleRandomWalkSO RoomGenerationParameters;
    protected override void RunProcedualGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions);

        _tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, _tilemapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions)
    {
        var currentPositions = _startPosition;

        for (int i = 0; i < _corridorCount; i++)
        {
            var corridor = WalkDungeunGenerator.RandomWalkCorridor(currentPositions, _corridorLength);
            currentPositions = corridor[corridor.Count - 1];

            floorPositions.UnionWith(corridor);
        }
    }
}
