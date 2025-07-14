using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed;
    protected Transform _target;

    public event Action<Enemy> Destroyed;
    public Rigidbody Rigidbody { get; private set; }

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    protected void OnTriggerEnter(Collider other)
    {
        Destroyed?.Invoke(this);
    }

    protected virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void Initialize(Transform target)
    {
        _target = target;
    }
}
