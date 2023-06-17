
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusteringManager : MonoBehaviour
{
    #region Singleton
    public static ClusteringManager Instance;
    private void Awake()
    {
        Instance = this;

        SpawnStartNPCManager.Instance.OnSpawningComplete += CanStartClustering;
    }
    #endregion

    public Action OnNewClustering;

    [SerializeField] private bool _userCanCluster = false;
    private void CanStartClustering()
    {
        _userCanCluster = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space") && _userCanCluster)
        {
            if (NPCManager.Instance.GetNPCount() < 7) return; // Can't cluster if not enough npcs exist

            Debug.Log("Do clustering");

            List<NPC>[] clustering;

            do
            {
                clustering = DoNPCClustering(NPCManager.Instance.GetCopyOfAllNPCs());
            }
            while (!AtLeastNNonEmptyClusters(clustering, 2)); // We need at least 2 tribes

            for(int i = 0; i < clustering.Length; i++)
            {
                foreach(NPC npc in clustering[i])
                {
                    npc.SetTribeID(i+1); // 0 is tribeless. Tribes start from 1
                }
            }

            OnNewClustering?.Invoke();
        }
    }

    private bool AtLeastNNonEmptyClusters(List<NPC>[] clusters, int n)
    {
        int clusterAmount = 0;

        for (int i = 0; i < clusters.Length; i++)
        {
            if (clusters[i].Count > 0)
            {
                clusterAmount++;
            }
        }

        return clusterAmount >= n;
    } 

    [SerializeField] private KMeansClustering _clustering;
    [SerializeField] private int[] _clusterContentForDebugging;

    public List<NPC>[] DoNPCClustering(List<NPC> toCluster, int clusterCount = 7)
    {
        Debug.Log("RANDOM POS: " + toCluster[0].GetComponent<Transform>().position);

        List<NPC>[] _result = _clustering.DoKMeans(toCluster);

        _clusterContentForDebugging = new int[clusterCount];
        for (int i = 0; i < _result.Length; i++)
        {
            _clusterContentForDebugging[i] = _result[i].Count;
        }

        return _result;
    }
}
