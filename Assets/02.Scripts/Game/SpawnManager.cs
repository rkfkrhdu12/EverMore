using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

public class SpawnManager : MonoBehaviour
{
    public void SetSpawnIndex(int index)
    {
        if (_isPlayer)
        {
            if (!_spawnAreaObject.activeSelf)
                _spawnAreaObject.SetActive(true);
        }

        _curSpawnIndex = index;
    }

    public void DestroyTopTower()
    {
        if (_spawnAreaChangedObject.activeSelf)
            _spawnAreaChangedObject.SetActive(false);

        _topSpawnAreaChangedObject.SetActive(false);
    }

    public void DestroyBottomTower()
    {
        if (_spawnAreaChangedObject.activeSelf)
            _spawnAreaChangedObject.SetActive(false);

        _bottomSpawnAreaChangedObject.SetActive(false);

    }


    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void Spawn()
    #region Function Content

    {
        if(_isGameEnd) { return; }

        UnitStatus uStatus = _teamUnits.GetUnit(_curSpawnIndex);

        if (_isPlayer)
        {
            if (_isUnitSpawn[_curSpawnIndex]) { return; }
            if (!_inGameSystem.CostConsumption(uStatus._cost)) { return; }

            _isUnitSpawn[_curSpawnIndex] = true;
            _costMgr.SetCoolDownUI(_curSpawnIndex, uStatus._coolTime);

            if (_spawnAreaObject.activeSelf)
                _spawnAreaObject.SetActive(false);
        }

        Vector3 unitPos = _spawnPoint;

        if (_spawnPoint == Vector3.zero)
        {
            int randVal = Random.Range(0, 2);
            bool isDown = 0 == randVal ? true : false;

            unitPos = isDown ?
                transform.position + new Vector3(1, 0, 2.5f) :
                transform.position + new Vector3(1, 0, .5f);
        }

        _curSpawnObject.transform.position = unitPos;

        UnitController unitCtrl = _curSpawnObject.GetComponent<UnitController>();

        unitCtrl._enemyCastleObject = _enemySpawnManager.GetCastle();

        if (unitCtrl._status != null)
            if (unitCtrl._status._equipedItems != null)
                UnitModelManager.Reset(_curSpawnObject.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl._status = uStatus;

        UnitModelManager.Update(_curSpawnObject.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl.Spawn();

        _curSpawnObject.SetActive(true);

        // 새로운 유닛 오브젝트를 오브젝트풀에서 찾음
        _curSpawnObject = null;
    }

    #endregion


    #region Variable
    [SerializeField]
    private SpawnManager _enemySpawnManager = null;
    public Team _teamUnits;

    [SerializeField]
    private GameObject _unitPrefabs = null;

    [SerializeField]
    private Vector3 _spawnPoint;

    [SerializeField]
    private GameObject _spawnAreaObject = null;

    [SerializeField]
    private GameObject _topSpawnAreaChangedObject = null;
    [SerializeField]
    private GameObject _bottomSpawnAreaChangedObject = null;
    [SerializeField]
    private GameObject _spawnAreaChangedObject = null;

    int _curSpawnIndex = 0;

    public void SetSpawnPoint(Vector3 pos) => _spawnPoint = pos;

    public Castle GetCastle() { return GetComponent<Castle>(); }

    public bool _isPlayer = false;

    List<GameObject> _unitList = new List<GameObject>();

    bool[] _isUnitSpawn = new bool[6];
    float[] _unitCoolIntervals = new float[6];
    float[] _unitCoolTimes = new float[6];

    GameObject _curSpawnObject = null;

    public InGameSystem _inGameSystem;

    public Transform _unitHealthBarList;
    public GameObject _unitHealthBarObject;

    public Canvas _canvas;

    public bool _isGameEnd = false;
    WaitForSeconds UpdateWaitTime = new WaitForSeconds(.25f);

    CostManager _costMgr;
    #endregion

    IEnumerator UpdateSpawnObject()
    {
        while (!_isGameEnd)
        {
            if (_curSpawnObject == null)
            {
                for (int i = 0; i < _unitList.Count; ++i)
                {
                    if (!_unitList[i].activeSelf)
                    {
                        _curSpawnObject = _unitList[i];
                        break;
                    }
                }

                if (_curSpawnObject == null)
                {
                    for (int j = 0; j < 6; ++j)
                    {
                        GameObject clone = Instantiate(_unitPrefabs, Vector3.zero, Quaternion.identity, null);

                        if(clone.activeSelf) { _isGameEnd = true; }

                        GameObject healthBar = Instantiate(_unitHealthBarObject, Vector3.zero, Quaternion.identity, _unitHealthBarList);

                        UnitController uc = clone.GetComponent<UnitController>();
                        uc._healthBarObject = healthBar;
                        uc._canvas = _canvas;
                        clone.name = (_isPlayer ? "Player1Unit " : "Player2Unit ") + _unitList.Count.ToString();

                        _unitList.Add(clone);
                    }

                    for (int i = 0; i < _unitList.Count; ++i)
                    {
                        if (!_unitList[i].activeSelf)
                        {
                            _curSpawnObject = _unitList[i]; break;
                        }
                    }
                }
            }
            else
            {
                yield return UpdateWaitTime;
            }
        }

        for (int i = 0; i < _unitList.Count; ++i)
        {
            _unitList[i].GetComponent<UnitController>()._isDead = true;
        }
    }

    IEnumerator UpdateUnitCoolTime()
    {
        while (!_isGameEnd)
        {
            for (int i = 0; i < _teamUnits.Length; ++i)
            {
                if (_isUnitSpawn[i])
                {
                    _unitCoolTimes[i] += Time.deltaTime;

                    if (_unitCoolIntervals[i] <= _unitCoolTimes[i])
                    {
                        _isUnitSpawn[i] = false;
                    }
                }
                else
                    yield return UpdateWaitTime;
            }
        }
    }

    #region Monobehaviour Function

    private void Awake()
    {
        GetComponent<FieldObject>()._team = _isPlayer ? eTeam.PLAYER : eTeam.ENEMY;

        StartCoroutine(UpdateSpawnObject());

        // Test
        if (!_isPlayer)
        {
            ItemList itemList = Manager.Get<GameManager>().itemList;

            // Test
            _teamUnits = new Team();
            _teamUnits.Init(eTeam.ENEMY);

            int[] items = new int[4];
            items[0] = itemList.CodeSearch(GameItem.eCodeType.Helmet, 1);
            items[1] = itemList.CodeSearch(GameItem.eCodeType.Bodyarmour, 1);
            items[3] = itemList.CodeSearch(GameItem.eCodeType.Weapon, 3);
            _teamUnits.SetEquipedItems(0, items);
            _teamUnits.GetUnit(0).UpdateItems();

            items = new int[4];
            items[0] = itemList.CodeSearch(GameItem.eCodeType.Helmet, 2);
            items[1] = itemList.CodeSearch(GameItem.eCodeType.Bodyarmour, 2);
            items[3] = itemList.CodeSearch(GameItem.eCodeType.Weapon, 4);
            _teamUnits.SetEquipedItems(1, items);
            _teamUnits.GetUnit(1).UpdateItems();

            items = new int[4];
            items[0] = itemList.CodeSearch(GameItem.eCodeType.Helmet, 3);
            items[1] = itemList.CodeSearch(GameItem.eCodeType.Bodyarmour, 3);
            items[2] = 303;
            items[3] = 303;
            _teamUnits.SetEquipedItems(2, items);
            _teamUnits.GetUnit(2).UpdateItems();
        }
    }

    private void OnEnable()
    {
        if(_spawnAreaObject.activeSelf)
        {
            _spawnAreaObject.SetActive(false);
        }
    }

    public void Enable(CostManager costMgr)
    {
        _costMgr = costMgr;

        for (int i = 0; i < _teamUnits.Length; ++i)
        {
            UnitStatus us = _teamUnits.GetUnit(i);
            us.UpdateItems();

            _isUnitSpawn[i] = false;
            _unitCoolIntervals[i] = us._coolTime;
            StartCoroutine(UpdateUnitCoolTime());
        }
    }

    private void Update()
    {
        // Test
        if (!_isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SetSpawnIndex(0);
                Spawn();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetSpawnIndex(1);
                Spawn();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                SetSpawnIndex(2);
                Spawn();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SetSpawnIndex(3);
                Spawn();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                SetSpawnIndex(4);
                Spawn();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SetSpawnIndex(5);
                Spawn();
            }
        }
    }

    #endregion

}
