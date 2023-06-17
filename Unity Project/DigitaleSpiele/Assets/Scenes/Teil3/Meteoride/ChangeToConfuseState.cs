using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToConfuseState : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            other.GetComponentInChildren<NPCStateConfused>().EnterThisState();
        }
    }
}
