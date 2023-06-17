using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimOnClustering : MonoBehaviour
{
    [SerializeField] private Transform _toAnimate;

    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeStrength = 10f;
    private Quaternion _startRot;

    private void Start()
    {
        ClusteringManager.Instance.OnNewClustering += StartAnimation;

        _startRot = transform.rotation;
    }

    private List<Tween> _currentAnimation = new();

    private void StartAnimation()
    {
        KillCurrentAnim();

        _currentAnimation.Add(_toAnimate.DOShakeRotation(_shakeDuration, _shakeStrength, 10, 40, true, ShakeRandomnessMode.Harmonic).OnKill(() => _toAnimate.rotation = _startRot));
    }

    private void KillCurrentAnim()
    {
        foreach (Tween tween in _currentAnimation)
        {
            if (tween != null && !tween.IsComplete())
            {
                tween.Kill();
            }
        }
        _currentAnimation = new();
    }
}
