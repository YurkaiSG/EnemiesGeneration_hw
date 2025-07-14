using UnityEngine;

public class Enemy2 : Enemy
{
    protected override void Move()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
    }
}
