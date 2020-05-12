using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSystem : MonoBehaviour
{
    public void Spawn(int unitNum)
    {
        //=> _unitMgr.Spawn(unitNum, ref _curGold);


    }

    #region Private Variable
    [SerializeField]
    private FieldObject _player1Base;

    [SerializeField]
    private Image _player1Bar;

    [SerializeField]
    private FieldObject _player2Base;

    [SerializeField]
    private Image _player2Bar;

    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _timerUITime;

    [SerializeField]
    private Image _goldImage;
    [SerializeField]
    private TextMeshProUGUI _goldText;

    private float _curGold;
    private const float _maxGold = 500;
    private const float _goldPerSecond = 10;

    [SerializeField]
    private Image _manaImage;
    [SerializeField]
    private TextMeshProUGUI _manaText;

    private float _curMana;
    private const float _maxMana = 100;
    private const float _manaPerSecond = 0.5f;

    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        if (null == Manager.Get<GameManager>().GetPlayerUnits()) { return; }

        _player1Base.GetComponent<SpawnManager>()._teamUnits = Manager.Get<GameManager>().GetPlayerUnits();

        _timerUITime = int.Parse(_timerText.text);
    }

    private void Update()
    {
        UpdateTimer();
        UpdateBase();
        UpdateGoldMana();
    }
    #endregion

    #region Private Function

    private void UpdateTimer()
    {
        _timerUITime -= Time.deltaTime;

        _timerText.text = $"{(int)_timerUITime}";
    }

    private void UpdateBase()
    {
        _player1Bar.fillAmount = _player1Base._curHp / _player1Base._maxHp;
        _player2Bar.fillAmount = _player2Base._curHp / _player2Base._maxHp;
    }

    private void UpdateGoldMana()
    {
        _curGold = Mathf.Clamp(_curGold + Time.deltaTime * _goldPerSecond, 0, _maxGold);
        _curMana = Mathf.Clamp(_curMana + Time.deltaTime * _manaPerSecond, 0, _maxMana);

        _goldText.text = ((int)_curGold).ToString();
        _manaText.text = ((int)_curMana).ToString();

        _goldImage.fillAmount = _curGold / _maxGold;
        _manaImage.fillAmount = _curMana / _maxMana;
    } 
    #endregion
}
