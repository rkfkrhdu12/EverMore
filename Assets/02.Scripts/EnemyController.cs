using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameplayIngredients;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private SpawnManager _spawnMgr;

    Stage.StageData _stageData;

    private void Awake()
    {
        _isSpawnEnd = true;
    }

    public void Enable()
    {
        _stageData = Manager.Get<GameManager>().GetEnemyUnitData();
        if(_stageData == null) { _isSpawnEnd = true; }

        _spawnMgr._teamUnits = _stageData.Team;
        _spawnMgr._teamUnits.UpdateItems();
        _spawnMgr.Enable();

        _curUnitData = _stageData._spawnUnitData[_curUnitDataNum];

        spawnTime = prevSpawnSec = _curUnitData.SpawnSec;

        _isSpawnEnd = false;
    }

    Stage.SpawnUnit _curUnitData;
    int _curUnitDataNum = 0;

    public Transform _topLineTrn; 
    public Transform _bottomLineTrn; 

    float spawnTime = 0.0f;
    float prevSpawnSec = 0.0f;

    bool _isSpawnEnd = false;

    private void FixedUpdate()
    {
        if (_isSpawnEnd) { return; }
        if (_stageData == null) { _isSpawnEnd = true; return; }

        spawnTime -= Time.fixedDeltaTime;
        if(spawnTime <= 0)
        {
            _spawnMgr.SetSpawnPoint(_curUnitData.IsDown ? _bottomLineTrn.position : _topLineTrn.position);
            _spawnMgr.SetSpawnIndex(_curUnitData.Index - 1);
            _spawnMgr.Spawn();

            if (_stageData._spawnUnitData.Count <= _curUnitDataNum + 1)
                _isSpawnEnd = true;

            _curUnitData = _stageData._spawnUnitData[++_curUnitDataNum];
            spawnTime = _curUnitData.SpawnSec - prevSpawnSec;
            prevSpawnSec = _curUnitData.SpawnSec;
        }
    }
}
