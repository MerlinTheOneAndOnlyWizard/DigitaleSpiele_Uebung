using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCState : MonoBehaviour
{
    [SerializeField] protected NPC _npc;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected NPCMoveTowardsTarget _moveTowardsTarget;
    [SerializeField] protected bool _stateActive = false;

    protected virtual void Awake()
    {
        _npc = GetComponentInParent<NPC>();
        _rb = GetComponentInParent<Rigidbody>();
        _moveTowardsTarget = GetComponentInParent<NPCMoveTowardsTarget>();
    }

    public virtual void EnterStateLogic()
    {
        _stateActive = true;
    }

    public virtual void ExitStateLogic()
    {
        _stateActive = false;
    }

    public virtual void DoNPCLogic()
    {

    }
}
