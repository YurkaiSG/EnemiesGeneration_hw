using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _spawnDelay = 2.0f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;
    private ObjectPool<Enemy> _pool;

    private void Start()
    {
        if (_spawnPoints.Length == 0)
        {
            Debug.LogError($"No spawnpoints assigned. Object name: {gameObject.name}.");
            return;
        }

        _pool = new ObjectPool<Enemy>(
            createFunc: () => CreateFunc(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );

        StartCoroutine(SpawnObject());
    }

    private Enemy CreateFunc()
    {
        Enemy enemy = Instantiate(_prefab);
        enemy.gameObject.SetActive(false);
        enemy.Initialize(transform.forward);
        return enemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        enemy.transform.position = spawnPoint.transform.position;
        enemy.transform.rotation = spawnPoint.transform.rotation;
        enemy.Destroyed += ReleaseOnTrigger;
        enemy.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.Destroyed -= ReleaseOnTrigger;
    }

    private void ReleaseOnTrigger(Enemy enemy)
    {
        _pool.Release(enemy);
    }

    private IEnumerator SpawnObject()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            _pool.Get();
            yield return wait;
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
    }
}
