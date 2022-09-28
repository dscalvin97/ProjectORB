using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public enum ScreenDirection { Top, Bottom, Left, Right }

public class EnemyManager : MonoBehaviour
{
    public float _SpawnRate = .5f;

    private Camera _cam;
    private Vector3 _minSpawnBound = new Vector3(-.1f, -.1f);
    private Vector3 _maxSpawnBound = new Vector3(1.1f, 1.1f);
    private GameManager _gameManager;
    private float _spawnTimer = 0;
    private bool _canSpawn = true;

    void Start()
    {
        _cam = Camera.main;
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_spawnTimer > 1 / _SpawnRate)
            _canSpawn = true;

        if (_canSpawn)
            SpawnEnemy();
        else
            _spawnTimer += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        ScreenDirection spawnDirection = (ScreenDirection) Random.Range(0, 3);
        Vector3 spawnVPPosition = Vector3.zero;

        switch (spawnDirection)
        {
            case ScreenDirection.Top:
                {
                    spawnVPPosition = new Vector3(Random.Range(_minSpawnBound.x, _maxSpawnBound.x), _maxSpawnBound.y);
                }
                break;
            case ScreenDirection.Bottom:
                {
                    spawnVPPosition = new Vector3(Random.Range(_minSpawnBound.x, _maxSpawnBound.x), _minSpawnBound.y);
                }
                break;
            case ScreenDirection.Left:
                {
                    spawnVPPosition = new Vector3(_minSpawnBound.x, Random.Range(_minSpawnBound.y, _maxSpawnBound.y));
                }
                break;
            case ScreenDirection.Right:
                {
                    spawnVPPosition = new Vector3(_maxSpawnBound.x, Random.Range(_minSpawnBound.y, _maxSpawnBound.y));
                }
                break;
        }
        RaycastHit hit;

        if (Physics.Raycast(_cam.ViewportPointToRay(spawnVPPosition), out hit, 100, LayerMask.GetMask("Ground")))
        {
            //Debug.Log(hit.point);
            _gameManager.pools[1].GetObjectFromPool(hit.point, Quaternion.identity);
        }
        _canSpawn = false;
        _spawnTimer = 0;
    }
}
