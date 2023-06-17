using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teil1GameCore : MonoBehaviour
{
    [SerializeField] private Transform _corner1Pos;
    [SerializeField] private Transform _corner2Pos;

    [SerializeField] private float _minSpawnCooldown = 1f;
    [SerializeField] private float _maxSpawnCooldown = 5f;

    [SerializeField] private float _cooldown;

    [SerializeField] private float _minSpawnSize = 0.5f;
    [SerializeField] private float _maxSpawnSize = 8f;


    [SerializeField] private GameObject _toSpawn;

    private void Awake()
    {
        _cooldown = ComputeSpawnCooldown();
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown < 0)
        {
            SpawnObject();
            _cooldown = ComputeSpawnCooldown();
        }
    }

    private void SpawnObject()
    {
        Vector3 pos1 = _corner1Pos.position;
        Vector3 pos2 = _corner2Pos.position;

        Vector3 pos = new Vector3(Random.Range(pos1.x, pos2.x), Random.Range(pos1.y, pos2.y), Random.Range(pos1.z, pos2.z));

        GameObject spawnedObject = Instantiate(_toSpawn, pos, Random.rotation);

        float scale = Random.Range(_minSpawnSize, _maxSpawnSize);
        spawnedObject.transform.localScale = new Vector3(scale, scale, scale);

        spawnedObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); // https://docs.unity3d.com/ScriptReference/Random.ColorHSV.html
    }

    private float ComputeSpawnCooldown()
    {
        return Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
    }

}
