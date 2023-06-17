using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCHotfix : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    private void FixedUpdate()
    {
        if (transform.position.y < 0)
        {
            _agent.Warp(new Vector3(transform.position.x, 0, transform.position.z));
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
