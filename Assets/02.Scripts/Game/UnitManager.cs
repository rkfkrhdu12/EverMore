using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;

//윤용우
public class UnitManager : MonoBehaviour
{
    #region Variable

    private GameObjectPool<UnitController> m_unitPool;
    private List<UnitController> m_unitList;

    // 유닛 생성
    // [SerializeField]
    // private CreateUnit _playerCreateUnit;

    [SerializeField]
    private GameObject _unitPrefab; //유닛 프리팹

    private readonly UnitController[] _units = new UnitController[UnitCount];

    public eTeam _eTeam;
    
    #endregion

    private static readonly int UnitCount = Team.UnitCount;

    protected void Start()
    {
        // 풀링
        int count = 0;
        m_unitPool = new GameObjectPool<UnitController>(20, () =>
        {
            var obj = Instantiate(_unitPrefab, transform, true);
            obj.name = $"Unit{++count}";
            obj.SetActive(false);
            var unit = obj.GetComponent<UnitController>();
            return unit;
        });
        m_unitList = new List<UnitController>();


        for (int i = 0; i < UnitCount; ++i)
        {
         //   _units[i] = Manager.Get<GameManager>().getPlayerUnit(i);
            _spawnInterval[i] = _spawnTime[i] = _units[i]._coolTime;
            _spawnCost[i] = _units[i]._cost;
        }
    }

    private void Update()
    {
        if (eTeam.ENEMY == _eTeam)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                CreateUnit(5);
            if (Input.GetKeyDown(KeyCode.X))
                CreateUnit(6);
        }

        for (int i = 0; i < UnitCount; ++i)
        {
            if (_spawnInterval[i] >= _spawnTime[i])
                _spawnTime[i] += Time.deltaTime;
        }
    }

    public float[] _spawnTime = new float[Team.UnitCount];
    public readonly float[] _spawnInterval = new float[Team.UnitCount];
    public readonly int[] _spawnCost = new int[Team.UnitCount];

    public void Spawn(int num, ref float gold)
    {
        if (!(_spawnInterval[num - 1] < _spawnTime[num - 1]) || !(_spawnCost[num - 1] <= gold)) return;
        
        _spawnTime[num - 1] = 0;
        gold -= _spawnCost[num - 1];

        CreateUnit(num);
    }

    #region Fuctions_Public

    public void CreateUnit(int num)
        // 유닛 생성
    {
        if (0 >= num || 6 < num)
        {
            Debug.LogError("CreateUnit() ");
            return;
        }

        var obj = m_unitPool.pop();

        //obj.transform.position = _playerCreateUnit.transform.position;

        var localPosition = obj.transform.localPosition;

        localPosition = new Vector3(localPosition.x,
            localPosition.y,
            localPosition.z + Random.Range(-4.5f, 4.0f));

        // var transform1 = obj.transform;
        //
        // transform1.localPosition = localPosition;
        // transform1.rotation = _playerCreateUnit.transform.rotation;

        obj.gameObject.SetActive(true);

        _units[num - 1]._team = _eTeam;

    //    obj.Init(_units[num - 1]);

        obj.Spawn();

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
