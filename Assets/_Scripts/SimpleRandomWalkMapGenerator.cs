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
            HashSet<Vector2Int> floorPositions = RunRandomWalk();
            _tilemapVisualizer.Clear();
            _tilemapVisualizer.PaintFloorTiles(floorPositions);
            WallGenerator.CreateWalls(floorPositions, _tilemapVisualizer);
        }

        protected HashSet<Vector2Int> RunRandomWalk()
        {
            var currentPosition = _startPosition;
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

            for (int i = 0; i < _randomWalkParameters.Iterations; i++)
            {
                var path = WalkDungeunGenerator.SimpleRandomWalk(currentPosition, _randomWalkParameters.WalkLength);

                floorPositions.UnionWith(path);

                if(_randomWalkParameters.StartRandomlyEachInteration)
                {
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
                }
            }

            return floorPositions;
        }

        
    }
}