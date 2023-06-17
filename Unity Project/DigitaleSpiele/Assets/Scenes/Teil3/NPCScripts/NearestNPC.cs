using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestNPC : MonoBehaviour
{
    [SerializeField] private NPC _npc;
    [SerializeField] private NPC _nearestNPC;

    private void Awake()
    {
        InvokeRepeating("UpdateNearestNPC", 0.1f, 0.5f);
    }

    private void UpdateNearestNPC()
    {
        _nearestNPC = NPCManager.Instance.GetNearestNPC(_npc, _npc.GetNPCTribeID(), _npc.transform.position, false);
    }

    public NPC GetNearestNPC()
    {
        return _nearestNPC;
    }
}
