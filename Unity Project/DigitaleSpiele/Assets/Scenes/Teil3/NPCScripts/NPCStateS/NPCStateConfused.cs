using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateConfused : NPCState
{
    [SerializeField] private NPCState _searchState;
    [SerializeField] private NPCState _wanderState;

    [SerializeField] private NPCMoveTowardsTarget _movement;

    [SerializeField] private float _stateDuration = 0;
    [SerializeField] private MaterialUpdate _updateMat;

    [SerializeField] private Animator[] _anim;

    [SerializeField] private GameObject _mouth;

    protected override void Awake()
    {
        base.Awake();

        _npc.OnTribeIdChanged += (int tribeID) => ExitState();       
    }

    private void Update()
    {
        if (_stateActive && _stateDuration > 0)
        {
            _stateDuration -= Time.deltaTime;

            if (_stateDuration <= 0)
            {
                ExitState();
            }
        }
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();

        ChangeMovementState(false);
        _updateMat.SetRedMaterial(true);
        SetAnimActive(false);
        _mouth.SetActive(false);
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ChangeMovementState(true);
        _updateMat.SetRedMaterial(false);
        _mouth.SetActive(true);

        SetAnimActive(true);
    }

    private void SetAnimActive(bool toActivate)
    {
        int _animSpeed = 0;
        if (toActivate) _animSpeed = 1;

        foreach (Animator anim in _anim)
        {
            if (!anim.gameObject.activeSelf) return;

            anim.speed = _animSpeed;
        }
    }

    public void EnterThisState(float duration = 3f)
    {
        _stateDuration = duration;

        _npc.ChangeState(this);
    }

    private void ExitState()
    {
        if (_npc.GetNPCTribeID() == 0)
        {
            _npc.ChangeState(_searchState);
        } else
        {
            _npc.ChangeState(_wanderState);
        }
    }
    
    private void ChangeMovementState(bool canMove)
    {
        _movement.SetCanMove(canMove);
    }
}
