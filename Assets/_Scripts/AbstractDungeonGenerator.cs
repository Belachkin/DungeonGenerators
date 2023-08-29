using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected TilemapVisualizer _tilemapVisualizer = null;
    [SerializeField] protected Vector2Int _startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        _tilemapVisualizer.Clear();
        RunProcedualGeneration();
    }

    protected abstract void RunProcedualGeneration();
}
