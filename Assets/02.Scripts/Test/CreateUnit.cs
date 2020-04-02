using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윤용우
public class CreateUnit : MonoBehaviour
{
    [SerializeField]
    GameObject _spawnPoint;

    public UnitData[] _units = null;

    protected int _curUnitCount = 0;
    protected const int _maxUnitCount = 10;

    public void SetUnit(UnitData[] units)
    {
        if (_maxUnitCount <= units.Length)  { Debug.LogErrorFormat("Unit 최대치 도달"); }
        else if (null == units)             { Debug.LogErrorFormat("Unit이 제대로 적용되지 않았습니다."); }

        _units = units;
        _curUnitCount = _units.Length;
    }

    protected void SpawnUnit(int num)
    {
        var o = gameObject;
        
        var unit = Instantiate(_units[num], o.transform.position, o.transform.rotation, null);
        unit.Spawn();
    }
}
