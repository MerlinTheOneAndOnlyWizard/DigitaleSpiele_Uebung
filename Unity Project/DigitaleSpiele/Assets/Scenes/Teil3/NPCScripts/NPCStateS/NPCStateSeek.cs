using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateSeek : NPCState
{
    [SerializeField] private NPCState _stateAfterBeingPartOfTribe;

    [SerializeField] private NearestNPC _nearestNPC;
    
    [SerializeField] private float _minCooldownToChangeTarget = 3f;
    [SerializeField] private float _maxCooldownToChangeTarget = 8f;
    [SerializeField] private float _cooldownToChangeTarget = -1f;

    [SerializeField] private float _minCooldownToFollowRandomDir = 3f;
    [SerializeField] private float _maxCooldownToFollowRandomDir = 8f;
    [SerializeField] private float _cooldownToFollowRandomDir = -1f;

    [SerializeField] private Transform _randomPos;

    [SerializeField] private float _maxDistanceToDifferntNPC = 3f;

    protected override void Awake()
    {
        base.Awake();
        _randomPos = new GameObject("SeeekState: NPC Targets for " + transform.parent.name).transform;

        _npc.OnTribeIdChanged += OnTribeChange;
        SpawnStartNPCManager.Instance.OnSpawningComplete += TargetRandomNPC;
    }

    private void Start()
    {
        if (_npc.GetNPCTribeID() != 0)
        {
            OnTribeChange(_npc.GetNPCTribeID());
            return;
        }
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
    }

    public override void DoNPCLogic()
    {
        base.DoNPCLogic();

        if (_nearestNPC.GetNearestNPC() != null && (_nearestNPC.GetNearestNPC().transform.position - transform.position).magnitude <= _maxDistanceToDifferntNPC) {
            MoveAwayFromClosestNPC();
            return;
        }

        if (_cooldownToFollowRandomDir > 0)
        {
            _cooldownToFollowRandomDir -= Time.deltaTime;

            if (_cooldownToFollowRandomDir <= 0 || _rb.velocity == Vector3.zero)
            {
                _cooldownToFollowRandomDir = 0;
                TargetRandomNPC();
            }
        }

        if (_cooldownToChangeTarget > 0)
        {
            _cooldownToChangeTarget -= Time.deltaTime;

            if (_cooldownToChangeTarget <= 0)
            {
                FollowRandomPos();
            }
        }
    }

    private void OnTribeChange(int newTribeID)
    {
        if (newTribeID != 0) // is part of a tribe
        {
            _npc.ChangeState(_stateAfterBeingPartOfTribe);
        }
    }

    private void TargetRandomNPC()
    {
        _cooldownToChangeTarget = Random.Range(_minCooldownToChangeTarget, _maxCooldownToChangeTarget);

        _moveTowardsTarget.MoveTowardsTarget(NPCManager.Instance.GetRandomNPC(_npc).transform);
    }

    private void FollowRandomPos()
    {
        _randomPos.position = SpawnStartNPCManager.Instance.GetRandomPositionOnMap();

        _moveTowardsTarget.MoveTowardsTarget(_randomPos);

        _cooldownToFollowRandomDir = Random.Range(_minCooldownToFollowRandomDir, _maxCooldownToFollowRandomDir);
    }

    private void MoveAwayFromClosestNPC()
    {
        Vector3 pos = transform.position;
        _randomPos.position = pos + (pos - _nearestNPC.GetNearestNPC().transform.position).normalized;

        _moveTowardsTarget.MoveTowardsTarget(_randomPos); ;
    }
}
