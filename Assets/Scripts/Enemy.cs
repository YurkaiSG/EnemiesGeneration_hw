using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Transform[] _waypoints;
    private int _currentWaypointIndex = 0;

    public event Action<Enemy> Destroyed;
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _currentWaypointIndex = 0;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_currentWaypointIndex < _waypoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].position, _speed * Time.deltaTime);

            if (transform.position == _waypoints[_currentWaypointIndex].position)
            {
                _currentWaypointIndex++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroyed?.Invoke(this);
    }

    public void Initialize(Transform[] waypoints)
    {
        _waypoints = waypoints;
    }
}
