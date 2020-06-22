using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct Base
{
    public FieldObject _baseObject;
    public SpawnManager _spawnMgr;

    public Image _healthBar;
}

public class InGameSystem : MonoBehaviour
{
    public bool CostConsumption(int consumCost) { return _costMgr.CostConsumption(consumCost); }
    public void OnLevelUp() { _levelMgr.OnLevelUp(); }

    #region Private Variable
    // SerializeField / public

    [SerializeField]
    private Base _red;

    [SerializeField]
    private Base _blue;

    [Space]

    [SerializeField]
    private LevelManager _levelMgr;

    [SerializeField]
    private CostManager _costMgr;

    [SerializeField]
    private TextMeshProUGUI _timerText = null;

    public GameObject _victoryObject;
    public GameObject _defeatObject;

    // private

    private float _timerUITime;

    bool _isGameEnd = false;
    bool _isPlayerRed = true;

    #region Get BaseData 
    private FieldObject RedBase { get { return _red._baseObject; } }
    private SpawnManager RedSpawnMgr { get { return _red._spawnMgr; } }
    private Image RedHealthBar { get { return _red._healthBar; } }

    private FieldObject BlueBase { get { return _blue._baseObject; } }
    private SpawnManager BlueSpawnMgr { get { return _blue._spawnMgr; } }
    private Image BlueHealthBar { get { return _blue._healthBar; } }
    #endregion

    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        SpawnManager playerSpawnMgr = RedSpawnMgr._isPlayer2 ? BlueSpawnMgr : RedSpawnMgr;

        playerSpawnMgr._teamUnits = Manager.Get<GameManager>().GetPlayerUnits();

        _levelMgr.Init(_costMgr);
        _costMgr.Init(playerSpawnMgr, _levelMgr._curLevelData);
    }

    private void OnEnable()
    {
        if (null == Manager.Get<GameManager>().GetPlayerUnits()) { return; }

        _victoryObject.SetActive(false);
        _defeatObject.SetActive(false);

        _timerUITime = int.Parse(_timerText.text);

        _costMgr.Enable();
        _levelMgr.Enable();
    }

    private void Update()
    {
        if (_isGameEnd) return;

        _timerUITime -= Time.deltaTime;

        _timerText.text = "" + ((int)_timerUITime).ToString();

        RedHealthBar.fillAmount = RedBase.RemainHealth;

        if (RedBase.IsDead)
        {
            if (!_isPlayerRed)  Victory();
            else                Defeat();
        }

        BlueHealthBar.fillAmount = BlueBase.RemainHealth;

        if (BlueBase.IsDead)
        {
            if (_isPlayerRed)   Victory();
            else                Defeat();
        }
    }
    #endregion

    #region Private Function

    void Victory()
    {
        _isGameEnd = true;

        RedSpawnMgr._isGameEnd = _isGameEnd;
        BlueSpawnMgr._isGameEnd = _isGameEnd;

        _victoryObject.SetActive(true);
    }

    void Defeat()
    {
        _isGameEnd = true;

        RedSpawnMgr._isGameEnd = _isGameEnd;
        BlueSpawnMgr._isGameEnd = _isGameEnd;

        _defeatObject.SetActive(true);
    }

    private void UpdateTimer()
    {
    }

    private void UpdateBase()
    {
    }

    #endregion
}
