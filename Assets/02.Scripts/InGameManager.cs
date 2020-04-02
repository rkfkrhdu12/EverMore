using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameManager : MonoBehaviour
{
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

    private float _timerTime;

    [SerializeField]
    private UnitManager _unitMgr;

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

    public void Spawn(int unitNum) =>
        _unitMgr.Spawn(unitNum, ref _curGold);

    private void Awake() =>
        _timerTime = int.Parse(_timerText.text);

    private void Update()
    {
        UpdateTimer();
        UpdateBase();
        UpdateGoldMana();

        // if (_enemyBase.IsDead)
        // {
        //     Manager.Get<GameManager>()._WinTeam = eTeam.PLAYER;
        //     Manager.Get<GameManager>().NextGoto("GameResultScene");
        // }
        //
        // if (_playerBase.IsDead)
        // {
        //     Manager.Get<GameManager>()._WinTeam = eTeam.ENEMY;
        //     Manager.Get<GameManager>().NextGoto("GameResultScene");
        // }
    }

    private void UpdateTimer()
    {
        _timerTime -= Time.deltaTime;

        _timerText.text = $"{_timerTime}";
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
}
