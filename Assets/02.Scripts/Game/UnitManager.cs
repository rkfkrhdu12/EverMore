using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//윤용우
public class UnitManager : SingletonMonoBehaviour<UnitManager>
{
    #region Variable
    GameObjectPool<Unit> m_unitPool;
    List<Unit> m_unitList;

    // 유닛 생성
    [SerializeField]
    CreateUnit _playerCreateUnit;

    [SerializeField]
    CreateUnit _enemyCreateUnit;
    // #

    [SerializeField]
    GameObject _unitPrefab; //유닛 프리팹

    Unit[] _units = new Unit[UnitCount];

    #endregion

    readonly static int UnitCount = Team.UnitCount;

    protected override void OnStart()
    {
        // 풀링
        int count = 0;
        m_unitPool = new GameObjectPool<Unit>(20, () =>
        {
            var obj = Instantiate(_unitPrefab);
            obj.transform.SetParent(transform);
            obj.name = string.Format("Unit{0}", ++count);
            obj.SetActive(false);
            var unit = obj.GetComponent<Unit>();
            return unit;
        });
        m_unitList = new List<Unit>();
        
        GameSystem gSystem = GameSystem.Instance;

        for (int i = 0; i < UnitCount; ++i)
        {
            _units[i] = gSystem.GetPlayerUnit(i);
            _spawnInterval[i] = _units[i]._coolTime;
            _spawnCost[i] = _units[i]._cost;
        }

    }

    void Update()
    {
        for (int i = 0; i < UnitCount; ++i)
        {
            if (_spawnInterval[i] >= _spawnTime[i])
            {
                _spawnTime[i] += Time.deltaTime;
            }
        }
    }

    public float[] _spawnTime       = new float[Team.UnitCount];
    public readonly float[] _spawnInterval   = new float[Team.UnitCount];
    public readonly int[] _spawnCost   = new int[Team.UnitCount];

    public void Spawn(int num, ref float gold)
    {
        if (_spawnInterval[num - 1] < _spawnTime[num - 1] && _spawnCost[num - 1] <= gold)
        {
            _spawnTime[num - 1] = 0;
            gold -= _spawnCost[num - 1];

            CreateUnit(num);
        }
    }

    #region Fuctions_Public
    public void CreateUnit(int num)
    // 유닛 생성
    {
        if (0 >= num || 6 < num) { Debug.LogError("CreateUnit() "); return; }

        var obj = m_unitPool.pop();

        obj.transform.position = _playerCreateUnit.transform.position;

        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,
            obj.transform.localPosition.y,
            obj.transform.localPosition.z + Random.Range(-4.5f, 4.0f));

        obj.transform.rotation = _playerCreateUnit.transform.rotation;

        obj.Init(_units[num - 1]);

        obj.Spawn();

        obj.gameObject.SetActive(true);

        m_unitList.Add(obj);
    }

    //public void ResetMonster(Unit monster)
    //// 리스트 삭제 안함
    //{
    //    monster.gameObject.SetActive(false);
    //    m_unitPool.push(monster);

    //}
    //public void RemoveMonster(Unit monster)
    //// 리스트 삭제
    //{
    //    ResetMonster(monster);
    //    m_unitList.Remove(monster);
    //}
    //public void DeleteAll()
    //// 살아있는 유닛 모두 삭제
    //{
    //    if (IsInvoking("CreateMonster"))
    //    {
    //        CancelInvoke("CreateMonster");
    //    }
    //    for (int i = 0; i < m_unitList.Count; i++)
    //    {
    //        ResetMonster(m_unitList[i]);
    //    }
    //    // 반복문의 카운트가 줄어서 나중에 리스트 삭제
    //    m_unitList.RemoveAll(unit => unit.IsDead == true);
    //}
    #endregion
}
