using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateWander : NPCState // = Flee logic
{
    [SerializeField] private NPCFindNearestHunter _nearestHunter;
    [SerializeField] private NPCHunterCount _hunterCount;

    [SerializeField] private NPCState _huntState;

    [SerializeField] private NearestNPC _nearestNPC;

    [SerializeField] private float _fleeFromHunterWeight;
    [SerializeField] private float _meetWithTribeWeight;
    [SerializeField] private float _moveAwayFromNearestNPCWeight;

    [SerializeField] private Transform _target;

    [SerializeField] private float _distanceWhereStartsFleeing = 7f;
    [SerializeField] private float _maxDistanceToNearestNPC = 3f;

    [SerializeField] private float _minWanderTime = 1f;
    [SerializeField] private float _maxWanderTime = 3f;
    [SerializeField] private float _wanderCooldown;

    protected override void Awake()
    {
        base.Awake();

        _target = new GameObject("NPC Targets for " + transform.parent.name).transform;
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
        _wanderCooldown = Random.Range(_minWanderTime, _maxWanderTime);
    }

    private void Update()
    {
        if (!_stateActive) return;

        if (_wanderCooldown > 0) _wanderCooldown -= Time.deltaTime;
        else if (_hunterCount.GetHunterAmount() <= 0) _npc.ChangeState(_huntState); // Enter Hunt-State only if not being currently hunted
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
    }

    public override void DoNPCLogic()
    {
        base.DoNPCLogic();

        if (!_stateActive) return;

        UpdateWeights();

        if (_fleeFromHunterWeight >= _meetWithTribeWeight && _fleeFromHunterWeight >= _moveAwayFromNearestNPCWeight)
        {
            Flee();
        } else if(_moveAwayFromNearestNPCWeight >= _meetWithTribeWeight && _moveAwayFromNearestNPCWeight >= _fleeFromHunterWeight)
        {
            RunAwayFromNearestNPC();
        } else
        {
            MeetWithTribe();
        }

        _moveTowardsTarget.MoveTowardsTarget(_target);
    }

    private void UpdateWeights()
    {
        _fleeFromHunterWeight = 0;
        _meetWithTribeWeight = 0;
        _moveAwayFromNearestNPCWeight = 0;

        if (_nearestHunter.GetNearestHunterDistance() <= _distanceWhereStartsFleeing)
        {
            _fleeFromHunterWeight = 1;
        } else if (_nearestNPC.GetNearestNPC() != null && (_nearestNPC.GetNearestNPC().transform.position - transform.position).magnitude <= _maxDistanceToNearestNPC) {
            _moveAwayFromNearestNPCWeight = 1;
        } else
        {
            _meetWithTribeWeight = 1;
        }
    }

    private void Flee()
    {
        Transform nearestHunter = _nearestHunter.GetNearestHunterPos();

        if (nearestHunter != null)
        {
            Vector3 pos = transform.position;

            _target.position = pos + (pos - nearestHunter.position).normalized;
        }
    }

    private void MeetWithTribe()
    {
        _target.position = NPCManager.Instance.GetTribeControid(_npc.GetNPCTribeID());
    }

    private void RunAwayFromNearestNPC()
    {
        Vector3 pos = transform.position;
        _target.position = pos + (pos - _nearestNPC.GetNearestNPC().transform.position).normalized;
    }
}
