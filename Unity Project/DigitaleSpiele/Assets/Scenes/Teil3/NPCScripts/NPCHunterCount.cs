using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHunterCount : MonoBehaviour
{
    [SerializeField] private int _hunterOnThisNPC;
    [SerializeField] private List<NPC> _hunterNPCSOnThisNPC;

    private void Awake()
    {
        _hunterNPCSOnThisNPC = new();
    }

    public void AddHunter(NPC newHunter)
    {
        if (_hunterNPCSOnThisNPC.Contains(newHunter)) return;

        _hunterNPCSOnThisNPC.Add(newHunter);
        _hunterOnThisNPC++;

        if (_hunterOnThisNPC == 1) OnBeingHunted?.Invoke();
    }

    public void RemoveHunter(NPC hunterToRemove)
    {
        if (!_hunterNPCSOnThisNPC.Remove(hunterToRemove))
        {
            //Debug.LogWarning("Removed" + hunterToRemove.gameObject.name + " twice!");
        } else
        {
            Mathf.Max(0, _hunterOnThisNPC--);
        }

        if (_hunterOnThisNPC == 0) OnStoppedBeingHunted?.Invoke();
    }

    public int GetHunterAmount()
    {
        return _hunterOnThisNPC;
    }

    public Action OnBeingHunted;
    public Action OnStoppedBeingHunted;
}
