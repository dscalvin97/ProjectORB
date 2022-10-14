using UnityEngine;

public enum ScreenDirection { Top, Bottom, Left, Right }

public class EnemyManager : MonoBehaviour
{
    public float _SpawnRate = .5f;
    public int _MaxEnemySpawnedCount = 50;

    private bool _canSpawn = true;
    private int _currentSpawnedEnemyCount = 0;
    private int _enemyPoolMaxCount = 0;
    private float _spawnTimer = 0;
    private Vector3 _minSpawnBound = new Vector3(-.1f, -.1f);
    private Vector3 _maxSpawnBound = new Vector3(1.1f, 1.1f);
    private Camera _cam;
    private GameManager _gameManager;
    private PlayerController _player;

    void Start()
    {
        _cam = Camera.main;
        _gameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<PlayerController>();
        _enemyPoolMaxCount = _gameManager.pools[1].poolCount;
    }

    // Update is called once per frame
    void Update()
    {
        _MaxEnemySpawnedCount = Mathf.CeilToInt(Mathf.Pow(_player._Level / .2f, 1.35f));
        _currentSpawnedEnemyCount = _enemyPoolMaxCount - _gameManager.pools[1].Count;

        if (_spawnTimer > 1 / _SpawnRate)
            _canSpawn = true;

        if (_canSpawn && _currentSpawnedEnemyCount < _MaxEnemySpawnedCount)
            SpawnEnemy();
        else
            _spawnTimer += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        _SpawnRate = Mathf.Clamp(_player._Level * .5f, 0, 10f);
        ScreenDirection spawnDirection = (ScreenDirection) Random.Range(0, 3);
        Vector3 spawnVPPosition = Vector3.zero;

        switch (spawnDirection)
        {
            case ScreenDirection.Top:
                    spawnVPPosition = new Vector3(Random.Range(_minSpawnBound.x, _maxSpawnBound.x), _maxSpawnBound.y);
                break;
            case ScreenDirection.Bottom:
                    spawnVPPosition = new Vector3(Random.Range(_minSpawnBound.x, _maxSpawnBound.x), _minSpawnBound.y);
                break;
            case ScreenDirection.Left:
                    spawnVPPosition = new Vector3(_minSpawnBound.x, Random.Range(_minSpawnBound.y, _maxSpawnBound.y));
                break;
            case ScreenDirection.Right:
                    spawnVPPosition = new Vector3(_maxSpawnBound.x, Random.Range(_minSpawnBound.y, _maxSpawnBound.y));
                break;
        }
        RaycastHit hit;

        if (Physics.Raycast(_cam.ViewportPointToRay(spawnVPPosition), out hit, 100, LayerMask.GetMask("Ground")))
        {
            Vector3 direction = (_player.transform.position - hit.point).normalized;
            //EnemyController newEnemy = (EnemyController) _gameManager.pools[1].GetObjectFromPool(hit.point, Quaternion.LookRotation(direction, Vector3.up));

            float maxHealth = Mathf.Pow(_player._Level / .2f, 1.25f) + 20;
            float characterSpeed = Mathf.Pow(Mathf.Log10(_player._Level * 2), Mathf.Epsilon) + 5.5f;
            float baseDamage = Mathf.Pow(Mathf.Log10(_player._Level * 2), Mathf.Epsilon) + 10f;
            EnemyController.CreateEnemy(_gameManager, hit.point, direction, maxHealth, characterSpeed, baseDamage);
        }
        _canSpawn = false;
        _spawnTimer = 0;
    }
}
