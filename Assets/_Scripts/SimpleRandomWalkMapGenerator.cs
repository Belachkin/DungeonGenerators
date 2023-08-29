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
               
        [SerializeField] private int _iterations = 10;
        public int WalkLength = 10;
        public bool StartRandomlyEachIteration = true;

        protected override void RunProcedualGeneration()
        {
            HashSet<Vector2Int> floorPositions = RunRandomWalk();

            _tilemapVisualizer.PaintFloorTiles(floorPositions);
        }

        protected HashSet<Vector2Int> RunRandomWalk()
        {
            var currentPosition = _startPosition;
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

            for (int i = 0; i < _iterations; i++)
            {
                var path = WalkDungeunGenerator.SimpleRandomWalk(currentPosition, WalkLength);

                floorPositions.UnionWith(path);

                if(StartRandomlyEachIteration)
                {
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
                }
            }

            return floorPositions;
        }

        
    }
}