using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFindNearestHunter : MonoBehaviour
{
    private NPC _npc;

    [SerializeField] private Transform _nearestHunterPos;
    [SerializeField] private float _distance;

    private void Awake()
    {
        _npc = GetComponentInParent<NPC>();

        InvokeRepeating("UpdateNearestHunter", 0.5f, 1f);
    }

    private void UpdateNearestHunter()
    {
        NPC hunter = NPCManager.Instance.GetNearestNPC(_npc, _npc.GetNPCTribeID(), _npc.transform.position, true);

        if (hunter != null)
        {
            _nearestHunterPos = hunter.transform;
            _distance = (transform.position - _nearestHunterPos.position).magnitude;
        } else
        {
            _nearestHunterPos = null;
            _distance = float.MaxValue;
        }
    }

    public Transform GetNearestHunterPos()
    {
        return _nearestHunterPos;
    }
    public float GetNearestHunterDistance()
    {
        return _distance;
    }
}
