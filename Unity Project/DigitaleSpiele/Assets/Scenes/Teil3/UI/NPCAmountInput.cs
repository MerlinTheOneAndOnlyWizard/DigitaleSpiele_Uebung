using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using DG.Tweening;

public class NPCAmountInput : MonoBehaviour
{
    [SerializeField] private bool _startedGame = false;
    [SerializeField] private int _npcAmountToSpawn;
    [SerializeField] private TextMeshProUGUI _inputField;

    [SerializeField] private GameObject _errorMessage;
    [SerializeField] private int _minInputAmount = 7;

    private void Awake()
    {
        _errorMessage.SetActive(false);
    }

    private void Start()
    {
        _inputField.GetComponentInParent<TMP_InputField>().Select();
    }

    void Update()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("enter") || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (_startedGame) return;

            bool validInput = UpdateNPCAmountWithTextField();
            if (!validInput) 
            { 
                Debug.Log("invalid input in text field");
                _errorMessage.SetActive(true);
                return; 
            }

            _startedGame = true;

            SpawnStartNPCManager.Instance.SpawnNPCs(_npcAmountToSpawn);

            transform.DOScale(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => gameObject.SetActive(false));
        }
    }

    /// <summary>
    /// Updates the npc amount to spawn
    /// </summary>
    /// <returns> True, if the user input is valid. Otherwise false.</returns>
    private bool UpdateNPCAmountWithTextField() 
    {
        string toParse = _inputField.text.Trim();
        int.TryParse(new string(toParse.Where(char.IsDigit).ToArray()), out _npcAmountToSpawn);
          
        return _npcAmountToSpawn >= _minInputAmount;
    }
}
