using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnit : MonoBehaviour
{
    public GameObject _spawnPoint;

    public Unit[] _units = null;

    protected int _curUnitCount = 0;
    protected const int _maxUnitCount = 10;

    public void SetUnit(Unit[] units)
    {
        if (_maxUnitCount <= units.Length)  { Debug.LogErrorFormat("Unit 최대치 도달"); }
        else if (null == units)             { Debug.LogErrorFormat("Unit이 제대로 적용되지 않았습니다."); }

        _units = units;
        _curUnitCount = _units.Length;
    }

    protected void SpawnUnit(int num)
    {
        Unit unit = Instantiate(_units[num], gameObject.transform.position, gameObject.transform.rotation, null);
        unit._isSpawn = true;
    }
}
