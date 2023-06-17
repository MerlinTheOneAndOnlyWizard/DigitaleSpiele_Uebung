using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStartNPCManager : MonoBehaviour
{
    #region Singleton
    public static SpawnStartNPCManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [SerializeField] private int _minAmount = 40;
    [SerializeField] private int _maxAmount = 50;

    [SerializeField] private Transform _corner1;
    [SerializeField] private Transform _corner2;

    [SerializeField] private float _spawnDelayPerUnitySpawned = 0.05f;

    public void SpawnNPCs(int amountToSpawn)
    {
        Vector3 p1 = _corner1.position;
        Vector3 p2 = _corner2.position;

        float spawnDelay = 0;

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 pos = GetRandomPositionOnMap(p1, p2);

            SpawnWithDelay(pos, spawnDelay);

            spawnDelay += _spawnDelayPerUnitySpawned;
        }
        StartCoroutine(DelayedAction(spawnDelay, () => OnSpawningComplete.Invoke()));
    }

    public Vector3 GetRandomPositionOnMap()
    {
        Vector3 p1 = _corner1.position;
        Vector3 p2 = _corner2.position;
        return new Vector3(UnityEngine.Random.Range(p1.x, p2.x), UnityEngine.Random.Range(p1.y, p2.y), UnityEngine.Random.Range(p1.z, p2.z));
    }

    public Vector3 GetRandomPositionOnMap(Vector3 p1, Vector3 p2)
    {
        return new Vector3(UnityEngine.Random.Range(p1.x, p2.x), UnityEngine.Random.Range(p1.y, p2.y), UnityEngine.Random.Range(p1.z, p2.z));
    }

    IEnumerator DelayedAction(float delay, Action OnComplete)
    {
        yield return new WaitForSeconds(delay);

        OnComplete?.Invoke();
    }

    private void SpawnWithDelay(Vector3 pos, float delay)
    {
        StartCoroutine(DelayedAction(delay, () => NPCManager.Instance.SpawnNPC(pos, 0)));
    }

    public Action OnSpawningComplete;
}
