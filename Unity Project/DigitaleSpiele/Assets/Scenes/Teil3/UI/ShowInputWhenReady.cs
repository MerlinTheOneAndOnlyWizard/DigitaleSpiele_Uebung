using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ShowInputWhenReady : MonoBehaviour
{
    [SerializeField] private GameObject _inputsObject;

    [SerializeField] private float _animDuration = 0.5f;

    private void Awake()
    {
        _inputsObject.SetActive(false);
    }

    private void Start()
    {
        SpawnStartNPCManager.Instance.OnSpawningComplete += ShowInputs;
    }

    public void ShowInputs()
    {
        _inputsObject.SetActive(true);
        Transform t = _inputsObject.transform;
        t.localScale = Vector3.zero;

        t.DOScale(1, _animDuration).SetEase(Ease.OutBack);
    }
}
