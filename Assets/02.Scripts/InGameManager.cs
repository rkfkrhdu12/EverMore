using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameManager : MonoBehaviour
{
    [SerializeField] FieldObject        _playerBase    = null;
    [SerializeField] Image              _playerBar     = null;

    [SerializeField] FieldObject        _enemyBase     = null;
    [SerializeField] Image              _enemyBar      = null;

    [SerializeField] TextMeshProUGUI    _timerText     = null;
    float _timerTime = 0;

    [SerializeField] UnitManager        _unitMgr       = null;

    [SerializeField] Image              _goldImage     = null;
    float _curGold = 0;
    float _maxGold = 500;
    float _goldPerSecond = 10;

    [SerializeField] Image              _manaImage     = null;
    float _curMana = 0;
    float _maxMana = 100;
    float _manaPerSecond = 0.5f;

    public void Spawn(int unitNum)
    {
        _unitMgr.Spawn(unitNum,ref _curGold);
    }

    void Awake()
    {
        _timerTime = int.Parse(_timerText.text);
    }

    void Update()
    {
        UpdateTimer();
        UpdateBase();
        UpdateGoldMana();

        if(_enemyBase.IsDead)
        {
            GameManager.Instance._WinTeam = eTeam.PLAYER;
            GameManager.Instance.NextScene("GameResultScene");
        }
        if (_playerBase.IsDead)
        {
            GameManager.Instance._WinTeam = eTeam.ENEMY;
            GameManager.Instance.NextScene("GameResultScene");
        }
    }

    void UpdateTimer()
    {
        _timerTime -= Time.deltaTime;

        _timerText.text = "" + ((int)_timerTime).ToString();
    }

    void UpdateBase()
    {
        _playerBar  .fillAmount   = _playerBase.HP / _playerBase.maxHP;
        _enemyBar   .fillAmount    = _enemyBase.HP  / _enemyBase.maxHP;
    }

    void UpdateGoldMana()
    {
        _curGold += Time.deltaTime * _goldPerSecond;
        _curMana += Time.deltaTime * _manaPerSecond;

        _goldImage.fillAmount = _curGold / _maxGold;
        _manaImage.fillAmount = _curMana / _maxMana;
    }
}
