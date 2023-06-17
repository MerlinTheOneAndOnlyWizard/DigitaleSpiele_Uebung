using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorideSpawner : MonoBehaviour
{
    // Idea press enter to create meteoride spawner
    [SerializeField] private Meteoride _meteor;
    [SerializeField] private Transform _spawnPosition;

    [SerializeField] private float _minMinHeight = 10f;
    [SerializeField] private float _maxMaxHeight = 15f;

    [SerializeField] private float _minDuration = 10f;
    [SerializeField] private float _maxDuration = 7f;
    [SerializeField] private float _duration;

    [SerializeField] private bool _spawned = false;

    private void Awake()
    {
        _duration = Random.Range(_minDuration, _maxDuration);
    }

    private void Update()
    {
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;

            if (_duration <= 0)
            {
                Spawn();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_spawned && other.CompareTag("NPC"))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (_spawned) return; 

        _spawned = true;
        Meteoride m = Instantiate(_meteor, _spawnPosition.position, Random.rotation);
        m.OnDestroyAction += () => Destroy(gameObject);
    }
}
