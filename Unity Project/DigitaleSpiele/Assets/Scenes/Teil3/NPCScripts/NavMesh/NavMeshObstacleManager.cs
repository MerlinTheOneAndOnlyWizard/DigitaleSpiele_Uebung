using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshObstacleManager : MonoBehaviour
{
    // Similar to https://www.youtube.com/watch?v=PxudQmMTLkg&ab_channel=LlamAcademy

    [SerializeField] private NPC _npc;
    [SerializeField] private bool _obstacleIsActive = false;

    [SerializeField] private NavMeshObstacle _obstacle;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _activationMagnitudeThreshold = 0.1f;
    [SerializeField] private float _debugLastMovementAmount = 0;

    [SerializeField] private float _obstacleNotAllowedCounter = 0f;

    private void Awake()
    {
        UpdateNavMeshObjects();
        _npc.OnTribeIdChanged += (int newTribe) => ObstacleNotAllowed(); // Cant activate Obstacle for 2 seconds after chanding tribe
    }

    private Vector3 _lastPos;

    private void Update()
    {
        if (_obstacleNotAllowedCounter > 0)
        {
            _obstacleNotAllowedCounter -= Time.deltaTime;
        } else
        {
            UpdateNavMeshObjects();
        }
    }

    private void ObstacleNotAllowed(float duration = 2f)
    {
        _obstacleNotAllowedCounter = duration;
        UpdateNavMeshObjects();
    }

    private void FixedUpdate()
    {
        Vector3 newPos = transform.position;
        bool wasActive = _obstacleIsActive;

        _debugLastMovementAmount = (_lastPos - newPos).magnitude;

        _obstacleIsActive = _debugLastMovementAmount < _activationMagnitudeThreshold;

        //Debug.Log(Time.time + " Magnitude is: " + ((_lastPos - newPos).magnitude ));


        _lastPos = newPos;

        if (wasActive != _obstacleIsActive)
        {
            UpdateNavMeshObjects();
        }
    }

    private void UpdateNavMeshObjects()
    {
        // First disable both to prevent bugs / error messages
        _agent.enabled = false;
        //_obstacle.enabled = false;

        StartCoroutine(UpdateObstacleAndAgentActivity());
    }

    private IEnumerator UpdateObstacleAndAgentActivity()
    {
        yield return null;
        bool obstacleActive = _obstacleIsActive && _obstacleNotAllowedCounter <= 0;

        //_obstacle.enabled = obstacleActive;
        _obstacle.carving = obstacleActive;

        _agent.enabled = !obstacleActive;
    }
}
