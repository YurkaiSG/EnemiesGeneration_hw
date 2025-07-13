using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _spawnDelay = 2.0f;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;
    private ObjectPool<Enemy> _pool;

    private void Start()
    {
        if (_wayPoints.Length == 0)
        {
            Debug.LogError($"No waypoints assigned. Object name: {gameObject.name}.");
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
        enemy.Initialize(_wayPoints);
        return enemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        enemy.transform.SetPositionAndRotation(transform.position, transform.rotation);
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
}
