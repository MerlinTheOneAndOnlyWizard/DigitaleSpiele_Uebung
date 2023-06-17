using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FancierUIText : MonoBehaviour
{
    [SerializeField] private Transform _textTransform;

    private Sequence _animationSequence;

    // Start is called before the first frame update
    void OnEnable()
    {
        _textTransform.localScale = Vector3.zero;

        CreateSequence();
    }

    private void CreateSequence() // Animated via Code for performance reasons (One should not animate UI with normal animations!)
    {
        _animationSequence = DOTween.Sequence();

        _animationSequence.Append(_textTransform.DOScale(1, 0.5f).SetEase(Ease.OutBack));
        _animationSequence.Append(_textTransform.DOScale(0, 0.5f).SetEase(Ease.OutQuint).SetDelay(2f));
    }
}
