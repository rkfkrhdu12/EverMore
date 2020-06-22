using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;
using UnityEditorInternal;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private SpawnManager _enemySpawnManager = null;
    public Team _teamUnits;

    [SerializeField]
    private GameObject _unitPrefabs = null;

    [SerializeField]
    private Vector3 _spawnPoint;

    int _curSpawnIndex = 0;

    public void SetSpawnIndex(int index) => _curSpawnIndex = index;

    public void SetSpawnPoint(Vector3 pos) => _spawnPoint = pos;
    public Castle GetCastle() { return GetComponent<Castle>(); }

    public bool _isPlayer2 = false;

    List<GameObject> _unitList = new List<GameObject>();

    GameObject _curSpawnObject = null;

    public InGameSystem _inGameSystem;

    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void Spawn()
    {
        UnitStatus uStatus = _teamUnits.GetUnit(_curSpawnIndex);

        if(!_inGameSystem.CostConsumption(uStatus._cost)) { return; }

        Vector3 unitPos = _spawnPoint;

        if(_spawnPoint == Vector3.zero)
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

        if (unitCtrl._status._equipedItems != null)
            UnitModelManager.Reset(_curSpawnObject.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl._status = uStatus;

        UnitModelManager.Update(_curSpawnObject.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl.Spawn();

        _curSpawnObject.SetActive(true);

        // 새로운 유닛 오브젝트를 오브젝트풀에서 찾음
        _curSpawnObject = null;
    }

    public bool _isGameEnd = false;
    WaitForSeconds ObjectUpdateWaitTime = new WaitForSeconds(.25f);
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
                    for (int j = 0; j < 3; ++j)
                    {
                        GameObject clone = Instantiate(_unitPrefabs, Vector3.zero, Quaternion.identity, null);

                        if(clone.activeSelf) { yield return null; }

                        clone.name = (_isPlayer2 ? "Player2Unit " : "Player1Unit ") + _unitList.Count.ToString();

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
                yield return ObjectUpdateWaitTime;
            }
        }

        for (int i = 0; i < _unitList.Count; ++i)
        {
            _unitList[i].GetComponent<UnitController>()._isDead = true;
        }
    }

    #region Monobehaviour Function

    private void Awake()
    {
        GetComponent<FieldObject>()._team = _isPlayer2 ? eTeam.ENEMY : eTeam.PLAYER;

        StartCoroutine(UpdateSpawnObject());

        // Test
        if (_isPlayer2)
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

            items = new int[4];
            items[0] = itemList.CodeSearch(GameItem.eCodeType.Helmet, 2);
            items[1] = itemList.CodeSearch(GameItem.eCodeType.Bodyarmour, 2);
            items[3] = itemList.CodeSearch(GameItem.eCodeType.Weapon, 4);
            _teamUnits.SetEquipedItems(1, items);
        }

        if(_teamUnits == null)
            _teamUnits = Manager.Get<GameManager>().GetPlayerUnits();

        for (int i = 0; i < _teamUnits.Length; ++i)
        {
            _teamUnits.GetUnit(i).UpdateItems();
        }
    }


    private void Update()
    {
        // Test
        if (_isPlayer2)
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
