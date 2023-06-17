using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class HurtCharacter : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private int _characterHealth = 3;

    [SerializeField] private TwoBoneIKConstraint _ik; 

    [SerializeField] private Collider _aliveCollider;
    [SerializeField] private Collider _deadCollider;

    [SerializeField] private GameObject _message;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _characterHealth--;

            _anim.SetTrigger("hurt");
           
            if (_characterHealth == 0)
            {
                _anim.SetBool("dead", true);

                DOTween.To(() => _ik.weight, x => _ik.weight = x, 0, 1);

                _aliveCollider.enabled = false;
                _deadCollider.enabled = true;

                _message.SetActive(true);
            }
        }
    }
}
