
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class KMeansClustering : MonoBehaviour
{
    public List<NPC>[] DoKMeans(List<NPC> toCluster, int clusterCount = 7, int iterations = 10)
    {
        List<NPC>[] clustering = InitialClustering(toCluster, clusterCount);

        for (int i = 0; i < iterations; i++)
        {
            Vector3[] centroids = ComputeCentroids(clustering, clusterCount);

            clustering = AssignToClusters(clustering, centroids, clusterCount);
        }

        return clustering;
    }

    private List<NPC>[] GetEmptyClustering(int clusterCount)
    {
        List<NPC>[] emptyClustering = new List<NPC>[clusterCount];

        for (int i = 0; i < clusterCount; i++)
        {
            emptyClustering[i] = new();
        }

        return emptyClustering;
    }

    #region Initialize Clusters
    private List<NPC>[] InitialClustering(List<NPC> toCluster, int clusterCount)
    {
        Assert.IsTrue(toCluster.Count >= clusterCount);

        // Use Forgy method -> Randomly Assign to clusters
        List<NPC>[] initialClustering = GetEmptyClustering(clusterCount);

        #region Assign each npc to a random cluster
        foreach (NPC npcToCluster in toCluster)
        {
            initialClustering[Random.Range(0, clusterCount)].Add(npcToCluster);
        }
        #endregion

        #region  Ensure, that every cluster has at least one element 
        bool noEmptyClusters = false;
        while(noEmptyClusters) 
        {
            int idxOfClusterWithNoElements = -1;
            int idxOfClusterWithMultipleElements = -1;
            for (int i = 0; i < clusterCount; i++)
            {
                if (idxOfClusterWithMultipleElements == -1 && initialClustering[i].Count > 0)
                {
                    idxOfClusterWithMultipleElements = i;
                    continue;
                }

                if (idxOfClusterWithNoElements == -1 && initialClustering[i].Count == 0)
                {
                    idxOfClusterWithNoElements = i;
                    continue;
                }
            }

            if (idxOfClusterWithNoElements == -1)
            {
                noEmptyClusters = true;
            } else
            {
                initialClustering[idxOfClusterWithNoElements].Add(initialClustering[idxOfClusterWithMultipleElements][0]);
                initialClustering[idxOfClusterWithMultipleElements].RemoveAt(0);
            }
        }
        #endregion

        return initialClustering;
    }
    #endregion

    #region Compute Centroids
    private Vector3[] ComputeCentroids(List<NPC>[] clustering, int clusterCount)
    {
        Vector3[] centroids = new Vector3[clusterCount]; 

        for (int i = 0; i < clusterCount; i++)
        {
            centroids[i] = ComputeControid(clustering[i]);
        }

        return centroids;
    }

    private Vector3 ComputeControid(List<NPC> cluster)
    {
        int amount = cluster.Count;
        Vector3 centroidSum = Vector3.zero;

        foreach(NPC npc in cluster)
        {
            centroidSum += npc.transform.position;
        }

        return (1f / amount) * centroidSum;
    }
    #endregion

    #region AssignNPCToCluster
    private List<NPC>[] AssignToClusters(List<NPC>[] toCluster, Vector3[] centroids, int clusterCount)
    {
        List<NPC>[] newClustering = GetEmptyClustering(clusterCount);

        for (int i = 0; i < clusterCount; i++)
        {
            foreach(NPC npcToCluster in toCluster[i])
            {
                int clusterIdx = ComputeNewClusterIdx(ComputeDistancesOfNPCToAllCentroids(npcToCluster, centroids, clusterCount), clusterCount);
                newClustering[clusterIdx].Add(npcToCluster);
            }
        }

        return newClustering;
    }
    
    private float[] ComputeDistancesOfNPCToAllCentroids(NPC npc, Vector3[] centroids, int clusterCount)
    {
        float[] distances = new float[clusterCount];

        for(int i = 0; i < clusterCount; i++)
        {
            distances[i] = (npc.transform.position - centroids[i]).magnitude;
        }

        return distances;
    }

    private int ComputeNewClusterIdx(float[] distances, int clusterCount)
    {
        int minDistanceIdx = 0;
        for (int i = 1; i < clusterCount; i++)
        {
            if (distances[i] < distances[minDistanceIdx])
            {
                minDistanceIdx = i;
            }
        }
        return minDistanceIdx;
    }
    #endregion
}
