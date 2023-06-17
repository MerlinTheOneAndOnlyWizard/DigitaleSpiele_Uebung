using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateHunt : NPCState
{
    [SerializeField] private NPC _targetNPC;
    [SerializeField] private NPCHunterCount _hunterCount;
    [SerializeField] private NPCState _wanderState;

    protected override void Awake()
    {
        base.Awake();

        _hunterCount.OnBeingHunted += EnterWander;
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();

        UpdateTarget();
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();

        _targetNPC?.RemoveHunter(_npc);
        _targetNPC = null;
    }

    public void EnterWander()
    {
        _npc.ChangeState(_wanderState);
    }


    public override void DoNPCLogic()
    {
        base.DoNPCLogic();

        if (!_stateActive) return;

        if (_targetNPC == null || _npc.GetNPCTribeID() == _targetNPC.GetNPCTribeID())
        {
            UpdateTarget();

            if (_targetNPC == null) EnterWander(); // No more NPCs of other tribes exist
        } 
    }


    private void UpdateTarget()
    {
        _targetNPC?.RemoveHunter(_npc);
        _targetNPC = NPCManager.Instance.GetNearestNPC(_npc, _npc.GetNPCTribeID(), transform.position, true);
        if (_targetNPC != null)
        {
            _moveTowardsTarget.MoveTowardsTarget(_targetNPC.transform);
            _targetNPC.AddHunter(_npc);
        }
    }
}

