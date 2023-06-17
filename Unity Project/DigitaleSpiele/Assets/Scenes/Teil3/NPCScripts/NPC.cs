using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour
{
    public Transform _transform;
    
    [SerializeField] private int _npcTribeID = 0; // each tribe has its own type. There are max 7 tribes (1-7). 0 means tribeless

    [SerializeField] private NPCState _currentState;
    [SerializeField] private MaterialUpdate _whiteFlash;

    private NPCGFXManager _gfx;

    #region Setup
    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        Setup();
    }
    public void SetupOnSpawn(int _npcTribeID)
    {
        SetTribeID(_npcTribeID);
        Setup();
    }

    private void Setup()
    {
        _gfx = GetComponentInChildren<NPCGFXManager>();
        _gfx.SetTribeGFX(_npcTribeID);
    }
    #endregion

    private void FixedUpdate()
    {
        _currentState.DoNPCLogic();
    }

    public void ChangeState(NPCState newState)
    {
        _currentState.ExitStateLogic();
        _currentState = newState;
        _currentState.EnterStateLogic();
    }


    #region TribeID
    public Action<int> OnTribeIdChanged;
    public void SetTribeID(int _npcTribeID)
    {
        this._npcTribeID = _npcTribeID;
        _whiteFlash.DoWhiteFlash();
        OnTribeIdChanged?.Invoke(_npcTribeID);
    }

    public int GetNPCTribeID()
    {
        return _npcTribeID;
    }

    public bool IsSameTribe(int npcTribeID)
    {
        return _npcTribeID == npcTribeID;
    }
    #endregion

    [SerializeField] private NPCStateHunt _hunterState;
    public bool IsHunter()
    {
        return (_currentState == _hunterState) ;
    }

    [SerializeField] private NPCHunterCount _hunterCount;
    public void AddHunter(NPC hunterToAdd)
    {
        _hunterCount.AddHunter(hunterToAdd);
    }
    public void RemoveHunter(NPC hunterToRemove)
    {
        _hunterCount.RemoveHunter(hunterToRemove);
    }
}
