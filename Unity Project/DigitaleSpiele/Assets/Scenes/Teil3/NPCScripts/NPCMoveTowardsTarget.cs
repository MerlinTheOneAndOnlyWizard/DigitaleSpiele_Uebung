using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoveTowardsTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _target;

    [SerializeField] private bool _canMove = true;
    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        if (_target == null)
        {
            _target = transform;
        }

        _navMeshAgent.destination = _target.position;
    }

    private void FixedUpdate()
    {
        //if (_navMeshAgent.isOnNavMesh && _navMeshAgent.isStopped != _canMove)
        //{
        //    _navMeshAgent.isStopped = _canMove;
        //}

        if (!_canMove) { 
            _rb.velocity = Vector3.zero; 
            return; 
        }

        if (_target != null)
        {
            SetDestination();
        }
    }

    public void MoveTowardsTarget(Transform target)
    {
        _target = target;
    }

    private void SetDestination()
    {
        if (_navMeshAgent.enabled && _navMeshAgent.isOnNavMesh && _navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.destination = _target.position;
        }
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }
}
