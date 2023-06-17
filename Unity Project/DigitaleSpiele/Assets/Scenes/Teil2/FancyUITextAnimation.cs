using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FancyUITextAnimation : MonoBehaviour
{
    [SerializeField] private Transform _textTransform;
    
    // Animation Step 1
    [SerializeField] private Transform _textScreenPosition;
    [SerializeField] private float _textMoveToScreenDuration = 0.4f;

    // Animation Step 2 -> Fancy Shake
    [SerializeField] private float _shakeDuration = 0.6f;
    [SerializeField] private float _shakeStrength = 5f;

    // Animation Step 3 -> Fancy Rotate
    [SerializeField] private float _rotateDuration = 0.6f;

    // Animation Step4 -> Fancy Leave
    [SerializeField] private float _leaveDuration = 0.6f;


    private Sequence _animationSequence;

    private void Start()
    {
        CreateSequence();

        _animationSequence.Play();
    }

    private void CreateSequence() // Animated via Code for performance reasons (One should not animate UI with normal animations!)
    {
        _animationSequence = DOTween.Sequence();

        float _startDelay = 0.5f;
        float _shakeStart = _startDelay + _textMoveToScreenDuration;
        
        // Move into screen
        _animationSequence.Append(_textTransform.DOLocalMove(_textScreenPosition.localPosition, _textMoveToScreenDuration).SetDelay(0.5f));
        
        // Do nice shake
        _animationSequence.Append(_textTransform.DOShakePosition(_shakeDuration, _shakeStrength, 5, 40, false, true, ShakeRandomnessMode.Harmonic));
        _animationSequence.Insert(_shakeStart, _textTransform.DOShakeRotation(_shakeDuration, _shakeStrength, 5, 40, true, ShakeRandomnessMode.Harmonic).OnComplete(() => _textTransform.rotation = Quaternion.identity));

        // Rotate 360°
        _animationSequence.Append(_textTransform.DORotate(new Vector3(0, 180, 0), _rotateDuration).SetDelay(5));

        // Good bye text!
        _animationSequence.Append(_textTransform.DOLocalMove(new Vector3(0, 1000, 0), _textMoveToScreenDuration).SetDelay(1));
    }
}
