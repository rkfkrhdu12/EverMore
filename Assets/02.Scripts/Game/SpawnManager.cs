﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _unitPrefabs = null;

    [SerializeField]
    private eTeam _curSpawnTeam = eTeam.PLAYER;

    [SerializeField]
    private Vector3 _spawnPoint;

    public Team _teamUnits;

    [SerializeField]
    private SpawnManager _enemySpawnManager;

    int _curSpawnIndex = 0;

    public void SetSpawnIndex(int index) => _curSpawnIndex = index;

    public void SetSpawnPoint(Vector3 pos) => _spawnPoint = pos;
    public Castle GetCastle() { return GetComponent<Castle>(); }

    /// <summary>
    /// UI의 버튼 혹은 설정된 키로 유닛을 스폰
    /// </summary>
    public void Spawn()
    {
        GameObject clone = Instantiate(_unitPrefabs, 
            (_spawnPoint != Vector3.zero ? _spawnPoint: transform.position + new Vector3(3,0,0)),
            Quaternion.identity, null);

        UnitController unitCtrl = clone.GetComponent<UnitController>();

        unitCtrl._enemyCastleObject = _enemySpawnManager.GetCastle();

        unitCtrl._status = _teamUnits.GetUnit(_curSpawnIndex);

        UnitModelManager.Update(clone.transform.GetChild(0).gameObject, unitCtrl._status._equipedItems);

        unitCtrl.Spawn();

        clone.SetActive(true);
    }
    
    #region Monobehaviour Function

    private void Awake()
    {
        GetComponent<FieldObject>()._team = _isPlayer2 ? eTeam.ENEMY : eTeam.PLAYER;

        if (_isPlayer2)
        {
            // Test
            _teamUnits = new Team();
            _teamUnits.Init(eTeam.ENEMY);

            int[] items = new int[4];
            items[0] = 3;
            items[1] = 4;
            items[3] = 9;
            _teamUnits.SetEquipedItems(0, items);

            items = new int[4];
            items[0] = 7;
            items[1] = 8;
            _teamUnits.SetEquipedItems(1, items);

            items = new int[4];
            items[0] = 27;
            items[1] = 28;
            _teamUnits.SetEquipedItems(2, items);

            items = new int[4];
            items[0] = 29;
            items[1] = 30;
            _teamUnits.SetEquipedItems(3, items);
        }
        else
            _teamUnits = Manager.Get<GameManager>().GetPlayerUnits();
    }

    public bool _isPlayer2 = false;

    private void Update()
    {
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
