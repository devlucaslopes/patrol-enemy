using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float Speed;
    public Transform[] Waypoints;

    [Header("Attack Settings")]
    public float AttackRange;
    public float TimeBetweenAttacks;

    private Animator anim;

    private int _waypointIndex = 0;
    private Transform _currentWaypoint;
    private int _direction = 1;

    private float _timeToNextAttack;

    private bool _canMove = true;
    private bool _canAttack = true;

    void Start()
    {
        anim = GetComponent<Animator>();

        _currentWaypoint = Waypoints[_waypointIndex];
        transform.position = _currentWaypoint.position;

    }

    void Update()
    {
        if (_canMove)
        {
            OnMove();
        }

        if (_canAttack)
        {
            GetPlayerToAttack();
        } else
        {
            Reloading();
        }
    }

    private void Reloading()
    {
        if (Time.time > _timeToNextAttack)
        {
            _canAttack = true;
            _canMove = true;
        }
    }

    private void GetPlayerToAttack()
    {
       RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * _direction, AttackRange);

        if (hit.collider != null)
        {
            _timeToNextAttack = Time.time + TimeBetweenAttacks;

            _canMove = false;
            _canAttack = false;

            anim.SetTrigger("attack");
            anim.SetBool("move", false);
        }
    }

    private void OnMove()
    {
        anim.SetBool("move", true);

        _currentWaypoint = Waypoints[_waypointIndex];

        transform.position = Vector2.MoveTowards(transform.position, _currentWaypoint.position, Speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _currentWaypoint.position) < 0.1f)
        {
            if (_waypointIndex < Waypoints.Length - 1)
            {
                _waypointIndex++;
            }
            else
            {
                _waypointIndex = 0;
            }
        }

        Flip();
    }

    private void Flip()
    {
        if (transform.position.x - _currentWaypoint.position.x > 0)
        {
            _direction = -1;

            transform.eulerAngles = new Vector3(0, 180, 0);
        } else
        {
            _direction = 1;
            transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, Vector3.right * _direction * AttackRange);
    }
}
