using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;
    private int _currentWaypointIndex = 0;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position == _waypoints[_currentWaypointIndex].position)
            _currentWaypointIndex = (_currentWaypointIndex +1) % _waypoints.Length;

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].position, _speed * Time.deltaTime);
    }
}
