using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlaneController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SpaceLogic();
        }
    }

    #region JumpLogic
    [SerializeField] private float _jumpPower = 6;
    [SerializeField] private float _jumpDuration = 0.5f;
    
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private Vector3 _shakeStrength;

    private List<Tween> _currentTween = new();
    private void SpaceLogic()
    {
        foreach(Tween tween in _currentTween)
        {
            if (tween != null && !tween.IsComplete()) tween.Kill();
        }

        _isJumping = true;

        Vector2 goalPos = transform.position;
        _currentTween.Add(transform.DOJump(goalPos, _jumpPower, 1, _jumpDuration, false).OnKill(() => transform.position = goalPos).OnComplete(() => _isJumping = false)); ;

        Quaternion rot = transform.rotation;
        _currentTween.Add(transform.DOShakeRotation(_shakeDuration, _shakeStrength, 10, 40, true, ShakeRandomnessMode.Harmonic).OnKill(() => transform.rotation = rot));
    }

    [SerializeField] private bool _isJumping = false;
    public bool IsJumping()
    {
        return _isJumping;
    }
    #endregion
}
