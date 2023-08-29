using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Scripts
{
    public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
    {

        [SerializeField] private SimpleRandomWalkSO _randomWalkParameters;

        protected override void RunProcedualGeneration()
        {
            HashSet<Vector2Int> floorPositions = RunRandomWalk(_randomWalkParameters);
            _tilemapVisualizer.Clear();
            _tilemapVisualizer.PaintFloorTiles(floorPositions);
            WallGenerator.CreateWalls(floorPositions, _tilemapVisualizer);
        }

        protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameter)
        {
            var currentPosition = _startPosition;
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

            for (int i = 0; i < parameter.Iterations; i++)
            {
                var path = WalkDungeunGenerator.SimpleRandomWalk(currentPosition, parameter.WalkLength);

                floorPositions.UnionWith(path);

                if(parameter.StartRandomlyEachInteration)
                {
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
                }
            }

            return floorPositions;
        }

        
    }
}