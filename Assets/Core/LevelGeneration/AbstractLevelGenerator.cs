using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractLevelGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateLevel()
    {
        RunProceduralGeneration();
    }

    public void ClearGeneratedLevel()
    {
        ClearLevel();
    }

    protected abstract void ClearLevel();
    protected abstract void RunProceduralGeneration();
}
