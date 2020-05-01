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
    private FieldObject _playerBase;

    [SerializeField]
    private Image _playerBar;

    [SerializeField]
    private FieldObject _enemyBase;

    [SerializeField]
    private Image _enemyBar;

    [SerializeField]
    private TextMeshProUGUI _timerText;
    private float _timerUITime;

    //[SerializeField]
    //private UnitManager _unitMgr;

    [SerializeField]
    private Image _goldImage;

    private float _curGold;
    private const float _maxGold = 500;
    private const float _goldPerSecond = 10;

    [SerializeField]
    private Image _manaImage;

    private float _curMana;
    private const float _maxMana = 100;
    private const float _manaPerSecond = 0.5f;

    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        if (null == Manager.Get<GameManager>().GetPlayerUnits()) { return; }

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

        _timerText.text = $"{_timerUITime}";
    }

    private void UpdateBase()
    {
        _playerBar.fillAmount = _playerBase._curHp / _playerBase._maxHp;
        _enemyBar.fillAmount = _enemyBase._curHp / _enemyBase._maxHp;
    }

    private void UpdateGoldMana()
    {
        _curGold += Time.deltaTime * _goldPerSecond;
        _curMana += Time.deltaTime * _manaPerSecond;

        _goldImage.fillAmount = _curGold / _maxGold;
        _manaImage.fillAmount = _curMana / _maxMana;
    } 
    #endregion
}
