using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteorideOnClick : MonoBehaviour
{
    // https://www.youtube.com/watch?v=5NTmxDSKj-Q&ab_channel=GameDevBeginner

    [SerializeField] private bool _canSpawn = false;
   
    void Start()
    {
        SpawnStartNPCManager.Instance.OnSpawningComplete += () => _canSpawn = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canSpawn)
        {
            if (!DoRaycast()) return;

            SpawnMeteor(_raycastHit.point);
        }
    }

    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private RaycastHit _raycastHit;
    private bool DoRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float maxDist = 100;
        return Physics.Raycast(ray.origin, ray.direction, out _raycastHit, maxDist, _raycastLayerMask);
    }

    [SerializeField] private MeteorideSpawner _meteor;
    private void SpawnMeteor(Vector3 pos)
    {
        Instantiate(_meteor, pos, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }
}

