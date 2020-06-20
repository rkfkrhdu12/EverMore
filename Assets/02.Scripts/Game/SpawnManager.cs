using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

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

    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void Spawn()
    {
        Vector3 unitPos = _spawnPoint;

        if(_spawnPoint == Vector3.zero)
        {
            int randVal = Random.Range(0, 2);
            bool isDown = 0 == randVal ? true : false;

            unitPos = isDown ?
                transform.position + new Vector3(1, 0, 2.5f) :
                transform.position + new Vector3(1, 0, .5f);
        }

        //GameObject clone = Instantiate(_unitPrefabs, unitPos, Quaternion.identity, null);

        _curSpawnObject.transform.position = unitPos;

        // clone.name = (_isPlayer2 ? "Player2Unit " : "Player1Unit ") + _spawnIndex++.ToString();

        UnitController unitCtrl = _curSpawnObject.GetComponent<UnitController>();
        
        unitCtrl._enemyCastleObject = _enemySpawnManager.GetCastle();

        if (unitCtrl._status._equipedItems != null)
            UnitModelManager.Reset(_curSpawnObject.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl._status = _teamUnits.GetUnit(_curSpawnIndex);
        unitCtrl._status.UpdateItems();

        UnitModelManager.Update(_curSpawnObject.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl.Spawn();

        _curSpawnObject.SetActive(true);

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
            // Test
            _teamUnits = new Team();
            _teamUnits.Init(eTeam.ENEMY);

            int[] items = new int[4];
            items[0] = 3;
            items[1] = 4;
            items[3] = 19;
            _teamUnits.SetEquipedItems(0, items);

            items = new int[4];
            items[0] = 5;
            items[1] = 6;
            items[3] = 17;
            _teamUnits.SetEquipedItems(1, items);
        }
        else
            _teamUnits = Manager.Get<GameManager>().GetPlayerUnits();
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
