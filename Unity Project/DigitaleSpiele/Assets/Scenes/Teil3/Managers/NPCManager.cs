using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    #region Singleton
    public static NPCManager Instance;
    private void Awake()
    {
        Instance = this;

        _existingNPCsByTribes = new List<NPC>[8];
        for(int i = 0; i < 8; i++)
        {
            _existingNPCsByTribes[i] = new();
        }

        _exisitingNPCsLengthByTribe = new int[8];

        _tribeCentroids = new Vector3[8];
    }
    #endregion

    [SerializeField] private NPC _toSpawnNPCObject;

    [SerializeField] private List<NPC>[] _existingNPCsByTribes;
    [SerializeField] private int[] _exisitingNPCsLengthByTribe;

    [SerializeField] private List<NPC> _existingNPCs = new();
    [SerializeField] private int _exisitingNPCsLength = 0;

    private void Start()
    {
        ClusteringManager.Instance.OnNewClustering += UpdateTribes;

        InvokeRepeating("UpdateAllTribeCentroids", 0.5f, 1f);
    }

    public void ChangeNPCTribe(NPC toChange, int newTribeID)
    {
        int oldTribeID = toChange.GetNPCTribeID();

        _exisitingNPCsLengthByTribe[oldTribeID]--;
        _existingNPCsByTribes[oldTribeID].Remove(toChange);

        _existingNPCsByTribes[newTribeID].Add(toChange);
        _exisitingNPCsLengthByTribe[newTribeID]++;

        toChange.SetTribeID(newTribeID);
    }

    public void SpawnNPC(Vector3 position, int tribeID)
    {
        NPC spawnedNPC = Instantiate(_toSpawnNPCObject, position, Quaternion.identity);
        
        spawnedNPC.SetupOnSpawn(tribeID);

        _existingNPCsByTribes[tribeID].Add(spawnedNPC);
        _existingNPCs.Add(spawnedNPC);

        _exisitingNPCsLength++;
        _exisitingNPCsLengthByTribe[tribeID] ++;

        spawnedNPC.gameObject.name += _exisitingNPCsLength.ToString();

        spawnedNPC.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360), 0);
    }

    public bool DespawnNPC(NPC toDespawn)
    {
        int tribeID = toDespawn.GetNPCTribeID();
        if (_existingNPCsByTribes[tribeID].Remove(toDespawn))
        {
            _existingNPCs.Remove(toDespawn);

            Destroy(toDespawn.gameObject);

            _exisitingNPCsLength--;
            _exisitingNPCsLengthByTribe[tribeID]--;

            return true;
        }
        return false;
    }

    private void UpdateTribes()
    {
        _existingNPCsByTribes = new List<NPC>[8];
        for (int i = 0; i < 8; i++)
        {
            _existingNPCsByTribes[i] = new();
        }
        _exisitingNPCsLengthByTribe = new int[8];

        foreach(NPC npc in _existingNPCs)
        {
            int tribe = npc.GetNPCTribeID();

            _existingNPCsByTribes[tribe].Add(npc);
            _exisitingNPCsLengthByTribe[tribe]++;
        }
    }

    public NPC GetNearestNPC(NPC notThis, int tribeID, Vector3 pos, bool fromDifferentTribe, bool hasToBeHunter = false)
    {
        float _nearestDistance = float.MaxValue;
        NPC _nearestNPC = null;

        for (int i = 0; i < 8; i++)
        {
            if (fromDifferentTribe && tribeID == i) continue;

            foreach (NPC tribeNPC in _existingNPCsByTribes[i])
            {
                if (tribeNPC == notThis) continue;
                if (hasToBeHunter && !tribeNPC.IsHunter()) continue;

                float dist = ((pos) - tribeNPC.transform.position).magnitude;

                if (dist <= _nearestDistance)
                {
                    _nearestDistance = dist;
                    _nearestNPC = tribeNPC;
                }
            }
        }

        return _nearestNPC;
    }

    public NPC GetRandomNPCOfDifferentTribe(int tribeIDNotFromTarget)
    {
        List<int> tribeIdxWithNPCs = new();
        for(int i = 0; i < 8; i++)
        {
            if (i != tribeIDNotFromTarget && _exisitingNPCsLengthByTribe[i] > 0)
            {
                tribeIdxWithNPCs.Add(i);
            }
        }

        int tribeCount = tribeIdxWithNPCs.Count;
        if (tribeCount == 0) return null;

        int randomTribeIdx = tribeIdxWithNPCs[Random.Range(0, tribeCount)];
        List<NPC> toExtractFrom = _existingNPCsByTribes[randomTribeIdx];

        int randomEnemyIdx = Random.Range((int)0, (int)_exisitingNPCsLengthByTribe[randomTribeIdx]);

        return toExtractFrom[randomEnemyIdx];
    }

    public NPC GetRandomNPC(NPC _notThis)
    {
        if (_exisitingNPCsLength <= 1) return null;

        NPC randomNPC;

        do
        {
            randomNPC = _existingNPCs[UnityEngine.Random.Range(0, _exisitingNPCsLength)];
        }
        while (randomNPC == _notThis);

        return randomNPC;
    }

    public int GetNPCount()
    {
        return _exisitingNPCsLength;
    }


    public List<NPC> GetCopyOfAllNPCs()
    {
        return new List<NPC>(_existingNPCs);
    }

    #region TribeCentroid
    [SerializeField] private Vector3[] _tribeCentroids;
    private void UpdateAllTribeCentroids()
    {
        for (int i = 0; i < 8; i++)
        {
            _tribeCentroids[i] = ComputTribeCentroid(i);
        }
    }

    private Vector3 ComputTribeCentroid(int tribeID)
    {
        Vector3 centroid = Vector3.zero;

        if (_exisitingNPCsLengthByTribe[tribeID] > 0)
        {
            foreach(NPC tribeNPC in _existingNPCsByTribes[tribeID])
            {
                centroid = tribeNPC.transform.position;
            }

            centroid /= _exisitingNPCsLengthByTribe[tribeID];   
        }

        return centroid;
    }

    public Vector3 GetTribeControid(int tribeID)
    {
        return _tribeCentroids[tribeID];
    }

    #endregion
}
