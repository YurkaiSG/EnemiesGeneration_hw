using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector3 _direction;

    public event Action<Enemy> Destroyed;
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _direction = new Vector3();
    }

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * _direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroyed?.Invoke(this);
    }

    public void Initialize(Vector3 direction)
    {
        _direction = direction;
    }
}
