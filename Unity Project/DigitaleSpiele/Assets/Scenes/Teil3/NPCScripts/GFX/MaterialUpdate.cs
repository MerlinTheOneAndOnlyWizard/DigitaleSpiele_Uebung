using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUpdate : MonoBehaviour
{
    private Dictionary<Renderer, Material> _defaultMaterial = new();

    private void Awake()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        
        foreach(Renderer rend in renderer)
        {
            _defaultMaterial.Add(rend, rend.material);
        }
    }

    #region WhiteFlash
    [SerializeField] private Material _whiteMaterial;
    [SerializeField] private float _flashDuration = 0.5f;
    [SerializeField] private float _flashCounter = 0;
    public void DoWhiteFlash()
    {
        if (_flashCounter <= 0)
        {
            SetAllRendererMaterial(false, _whiteMaterial);
        }

        _flashCounter = _flashDuration;
    }
    #endregion

    #region RedRegion
    [SerializeField] private Material _redMaterial;
    public void SetRedMaterial(bool redActive)
    {
        if (!redActive)
        {
            SetAllRendererMaterial(true, null);
        } else
        {
            SetAllRendererMaterial(false, _redMaterial);
        }
    }

    #endregion

    private void Update()
    {
        if (_flashCounter > 0)
        {
            _flashCounter -= Time.deltaTime;

            if (_flashCounter <= 0)
            {
                SetAllRendererMaterial(true, null);
            }
        }
    }

    private void SetAllRendererMaterial(bool setToDefaultMaterial, Material materialToSet)
    {
        if (setToDefaultMaterial)
        {
            foreach (KeyValuePair<Renderer, Material> pair in _defaultMaterial)
            {
                pair.Key.material = pair.Value;
            }
        } else
        {
            foreach (KeyValuePair<Renderer, Material> pair in _defaultMaterial)
            {
                pair.Key.material = materialToSet;
            }
        }
    }
}
