using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteoride : MonoBehaviour
{
    [SerializeField] private bool _exploded = false;
    [SerializeField] private ParticleSystem[] _explosions;

    [SerializeField] private float _cooldownUntilDestruction = 200f;

    [SerializeField] private ChangeToConfuseState _colliderToSpawn;

    private void Update()
    {
        if (_cooldownUntilDestruction > 0)
        {
            _cooldownUntilDestruction -= Time.deltaTime;

            if (_cooldownUntilDestruction <= 0 && !_exploded)
            {
                MeteorHitsGround();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!_exploded && collision.gameObject.CompareTag("Untagged"))
        {
            MeteorHitsGround();
        }
    }

    private void MeteorHitsGround()
    {
        _exploded = true;

        ParticleSystem p = Instantiate(_explosions[UnityEngine.Random.Range(0, _explosions.Length)], transform.position, Quaternion.identity);
        p.Play();

        Destroy(Instantiate(_colliderToSpawn, transform.position, Quaternion.identity), 0.5f);

        OnDestroyAction?.Invoke();
        Destroy(gameObject);
    }

    public Action OnDestroyAction;
}

