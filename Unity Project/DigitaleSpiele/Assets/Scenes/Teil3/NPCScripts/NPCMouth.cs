using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouth : MonoBehaviour
{
    [SerializeField] private GameObject _eatParticles;

    private NPC _npc;

    private void Awake()
    {
        _npc = GetComponentInParent<NPC>();
    }

    [SerializeField] private float _minCooldown = 0.5f;
    [SerializeField] private float _maxCooldown = 0.8f;
    [SerializeField] private float _cooldown;
    private void Update()
    {
        if (_cooldown >= 0)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("NPC")) return;

        if (_npc.GetNPCTribeID() == 0) return; // Can't eat if no tribe member;

        if (_cooldown > 0) return;

        int tribeID = _npc.GetNPCTribeID();

        NPC otherNPC = other.GetComponent<NPC>();
        if (!otherNPC.IsSameTribe(tribeID)) // different tribe -> EAT HIM!
        {
            _cooldown = Random.Range(_minCooldown, _maxCooldown);

            NPCManager.Instance.ChangeNPCTribe(otherNPC, tribeID);

            GameObject particle = Instantiate(_eatParticles, otherNPC.transform.position + Vector3.up*5, Quaternion.Euler(new Vector3(-90, 0, 0)));
            //Destroy(particle, 10f);
        }
    }
}
