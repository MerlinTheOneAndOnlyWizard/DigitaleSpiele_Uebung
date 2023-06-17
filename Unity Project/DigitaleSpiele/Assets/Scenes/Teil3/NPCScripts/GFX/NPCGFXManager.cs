using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPCGFXManager : MonoBehaviour
{
    private NPC _npc;

    private void Awake()
    {
        _npc = GetComponentInParent<NPC>();
        _npc.OnTribeIdChanged += SetTribeGFX;
    }

    [SerializeField] private List<GameObject> _gfx = new();

    public void SetTribeGFX(int tribeID)
    {
        Assert.IsTrue(tribeID <= 7 && tribeID >= 0);

        for (int i = 0; i < 8; i++)
        {
            if (i != tribeID)
            {
                _gfx[i].SetActive(false);
            } else
            {
                _gfx[i].SetActive(true);
            }
        }        
    }
}
