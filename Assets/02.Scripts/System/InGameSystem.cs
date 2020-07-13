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

        _levelMgr.Init(_costMgr);
        _costMgr.Init(_levelMgr);
    }

    private void OnEnable()
    {
        StartCoroutine(StartLoading());
        
        _victoryObject.SetActive(false);
        _defeatObject.SetActive(false);

        _timerUITime = int.Parse(_timerText.text);
    }

    [SerializeField] GameObject _loadingScreenObject = null;
    [SerializeField] Image _loadingBarImage = null;

    WaitForSeconds _loadingScreenTime1 = new WaitForSeconds(.2f);
    WaitForSeconds _loadingScreenTime2 = new WaitForSeconds(.2f);
    WaitForSeconds _loadingScreenTime3 = new WaitForSeconds(.1f);
    IEnumerator StartLoading()
    {
        if (_loadingScreenObject == null) yield break;

        int i = 0;

        if (!_loadingScreenObject.gameObject.activeSelf)
            _loadingScreenObject.gameObject.SetActive(true);

        _loadingBarImage.fillAmount = 0.0f;

        yield return _loadingScreenTime1;
        _loadingBarImage.fillAmount += .7f;

        yield return _loadingScreenTime2;
        _loadingBarImage.fillAmount += .3f;

        yield return _loadingScreenTime3;

        if (_loadingScreenObject.gameObject.activeSelf)
            _loadingScreenObject.gameObject.SetActive(false);

        SpawnManager playerSpawnMgr = RedSpawnMgr;
        EnemyController enemyController = BlueSpawnMgr.GetComponent<EnemyController>();

        playerSpawnMgr.Enable();
        enemyController.Enable();
        _costMgr.Enable(playerSpawnMgr);
        _levelMgr.Enable();

        _isEndLoad = true;
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

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_audio.isPlaying)
                _audio.Stop();
            else
                _audio.Play();
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

    #endregion
}
