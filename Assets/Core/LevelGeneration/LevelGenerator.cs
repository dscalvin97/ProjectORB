using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class LevelGenerator : AbstractLevelGenerator
{
    [SerializeField]
    private int iterations = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;

    public GameObject _Floor;
    public GameObject _WallF;
    public GameObject _WallB;
    public GameObject _WallL;
    public GameObject _WallR;
    public GameObject _CornerBL;
    public GameObject _CornerBR;
    public GameObject _CornerFL;
    public GameObject _CornerFR;
    public GameObject _CorridorH;
    public GameObject _CorridorV;

    private List<GameObject> tileObjectInstances = new List<GameObject>();

    private void Start()
    {
        RunProceduralGeneration();
    }

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(startRandomlyEachIteration);

        floorPositions = CleanLevelMap(floorPositions);

        ClearLevel();
        PlaceFloorTiles(floorPositions);
    }

    private HashSet<Vector2Int> CleanLevelMap(HashSet<Vector2Int> floorPositions)
    {
        int invalidCellsFound = 10;
        
        while (invalidCellsFound != 0)
        {
            invalidCellsFound = 0;
            foreach (Vector2Int cell in floorPositions.ToList())
            {
                int requiredWallCount = 0;

                foreach (var direction in Direction2D.cardinalDirectionsList)
                {
                    var neighbourPosition = cell + direction;
                    if (floorPositions.Contains(neighbourPosition) == false)
                        requiredWallCount++;
                }
                if (requiredWallCount >= 3)
                {
                    floorPositions.Remove(cell);
                    invalidCellsFound++;
                }
            }
        }
        return floorPositions;
    }

    private void PlaceFloorTiles(HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int cell in floorPositions)
        {
            List<Vector2Int> wallDirection = new List<Vector2Int>();
            GameObject cellObject = null;

            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPosition = cell + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallDirection.Add(direction);
                }
            }

            GameObject tileObject = null;
            if (wallDirection.Count == 2)
            {
                if (wallDirection.Contains(Direction2D.Up) && wallDirection.Contains(Direction2D.Right))
                    tileObject = _CornerFR;
                else if (wallDirection.Contains(Direction2D.Down) && wallDirection.Contains(Direction2D.Right))
                    tileObject = _CornerBR;
                else if (wallDirection.Contains(Direction2D.Down) && wallDirection.Contains(Direction2D.Left))
                    tileObject = _CornerBL;
                else if (wallDirection.Contains(Direction2D.Up) && wallDirection.Contains(Direction2D.Left))
                    tileObject = _CornerFL;
                else if (wallDirection.Contains(Direction2D.Up) && wallDirection.Contains(Direction2D.Down))
                    tileObject = _CorridorH;
                else if (wallDirection.Contains(Direction2D.Left) && wallDirection.Contains(Direction2D.Right))
                    tileObject = _CorridorV;
            }
            else if (wallDirection.Count == 1)
            {
                if (wallDirection.Contains(Direction2D.Up))
                    tileObject = _WallF;
                else if (wallDirection.Contains(Direction2D.Right))
                    tileObject = _WallR;
                else if (wallDirection.Contains(Direction2D.Down))
                    tileObject = _WallB;
                else if (wallDirection.Contains(Direction2D.Left))
                    tileObject = _WallL;
            }
            else
            {
                tileObject = _Floor;
            }

            if (tileObject != null)
            {
                cellObject = Instantiate(tileObject, new Vector3(cell.x * 3, 0, cell.y * 3), Quaternion.Euler(Vector3.zero), transform);
                tileObjectInstances.Add(cellObject);
            }
        }
    }

    protected override void ClearLevel()
    {
        if (tileObjectInstances.Count > 0)
        {
            foreach (GameObject cellObject in tileObjectInstances)
            {
                DestroyImmediate(cellObject);
            }
        }
        tileObjectInstances.Clear();
    }

    protected HashSet<Vector2Int> RunRandomWalk(bool startRandomlyEachIteration)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
