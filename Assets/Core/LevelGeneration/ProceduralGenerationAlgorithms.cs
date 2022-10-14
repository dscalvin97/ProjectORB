using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{

    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPosition = startPosition;
        Vector2Int previousDirection = Vector2Int.zero;

        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int direction = Direction2D.GetRandomCardinalDirection();

            while (direction == previousDirection)
            {
                direction = Direction2D.GetRandomCardinalDirection();
            }

            var newPosition = previousPosition + direction;
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), // UP
        new Vector2Int(1, 0), // RIGHT
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0) // LEFT
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }

    public static Vector2Int Up = new Vector2Int(0, 1);
    public static Vector2Int Right = new Vector2Int(1, 0);
    public static Vector2Int Down = new Vector2Int(0, -1);
    public static Vector2Int Left = new Vector2Int(-1, 0);
}