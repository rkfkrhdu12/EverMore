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
    public void Spawn(int index) { _playerCtrl.Spawn(index); }
    public bool CostConsumption(int consumCost) { return _costMgr.CostConsumption(consumCost); }
    public void OnLevelUp() { _levelMgr.OnLevelUp(); }

    public void OnGameEnd()
    {
        Manager.Get<SceneManagerPro>().LoadScene("MainScene");
    }

    public void OnUnitSpawnDown(GameObject obj)
    {
        obj.transform.localScale = new Vector3(.8f, .8f, .8f);
    }

    public void OnUnitSpawnUp(GameObject obj)
    {
        obj.transform.localScale = new Vector3(.9f, .9f, .9f);
    }

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

    public Canvas _canvas;

    // private

    PlayerController _playerCtrl;

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
        _isPlayerRed = RedSpawnMgr._isPlayer;

        SpawnManager playerSpawnMgr = _isPlayerRed ? RedSpawnMgr: BlueSpawnMgr;
        playerSpawnMgr._teamUnits = Manager.Get<GameManager>().GetPlayerUnits();

        playerSpawnMgr._canvas = _canvas;

        _playerCtrl = playerSpawnMgr.GetComponent<PlayerController>();

        playerSpawnMgr._teamUnits = Manager.Get<GameManager>().GetPlayerUnits();

        _levelMgr.Init(_costMgr);
        _costMgr.Init(_levelMgr);
    }

    private void OnEnable()
    {
        _victoryObject.SetActive(false);
        _defeatObject.SetActive(false);

        _timerUITime = int.Parse(_timerText.text);

        SpawnManager playerSpawnMgr = _isPlayerRed ? RedSpawnMgr : BlueSpawnMgr;

        playerSpawnMgr.Enable();
        _costMgr.Enable(playerSpawnMgr);
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

    [SerializeField]      Animator _victoryAnim;
    [SerializeField] Animator _defeatAnim;


    void Victory()
    {
        _isGameEnd = true;

        RedSpawnMgr._isGameEnd = _isGameEnd;
        BlueSpawnMgr._isGameEnd = _isGameEnd;

        _victoryObject.SetActive(true);
        //_victoryAnim.Play();
    }

    void Defeat()
    {
        _isGameEnd = true;

        RedSpawnMgr._isGameEnd = _isGameEnd;
        BlueSpawnMgr._isGameEnd = _isGameEnd;

        _defeatObject.SetActive(true);
        //_defeatAnim.Play();
    }

    #endregion
}
