using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemAbilityManager
{
    Dictionary<int, ItemAbility> _abilityList = new Dictionary<int, ItemAbility>();

    public ItemAbility GetAbility(int Code)
    {
        if (_abilityList.Count == 0) { Init(); }
        if (!_abilityList.ContainsKey(Code)) { return null; }

        return _abilityList[Code];
    }

    private void Init()
    {
        var abilityDatas = CSVParser.Read("AbilityTable");

        List<ItemAbility> abilityList = new List<ItemAbility>();

        abilityList.Add(new ItemAbilitySharpness());
        abilityList.Add(new ItemAbilitySharpness());
        abilityList.Add(new ItemAbilityFrostyFeet());

        int index = 0;
        foreach (var splitDatas in abilityDatas.Select(t => t.Split(',')))
        {
            if (string.IsNullOrWhiteSpace(splitDatas[0])) { break; }
            if (index >= abilityList.Count) { break; }

            int     .TryParse(splitDatas[0], out abilityList[index]._code);
            abilityList[index]._name = splitDatas[1];
            float   .TryParse(splitDatas[2], out abilityList[index]._range);
            float   .TryParse(splitDatas[3], out abilityList[index]._timeInterval);
            float   .TryParse(splitDatas[4], out abilityList[index]._condition);

            float var;

            float   .TryParse(splitDatas[5], out var);
            if (var != 0) abilityList[index]._variables.Add(var);

            float   .TryParse(splitDatas[6], out var);
            if (var != 0) abilityList[index]._variables.Add(var);

            _abilityList.Add(int.Parse(splitDatas[0]), abilityList[index++]);
        }
    }
}

public class ItemAbility
{
    // Public

    public int _code;                                   public int Code => _code;

    public string _name;                                public string Name => _name;

    public float _range;                                public float Range => _range;
    public float _timeInterval;                         public float Time => _timeInterval;
    public float _condition;                            public float Condition => _condition;
    public List<float> _variables = new List<float>();  public List<float> Var => _variables;

    // Private

    protected UnitStatus _us;
    protected UnitController _uCtrl;
    protected float _time;

    // Function

    /// <summary>
    /// 딱 한번만 작동 / 스탯 업데이트
    /// </summary>
    virtual public void Awake(UnitStatus us)
    {
        _us = us;
    }

    /// <summary>
    /// 스폰되고 출발하기전 작동 유닛이 꺼져있는 상태
    /// </summary>
    virtual public void Enable() { }

    /// <summary>
    /// 유닛이 스폰될때마다 작동 유닛이 켜지는 상태
    /// </summary>
    /// <param name="uCtrl"></param>
    virtual public void Start(UnitController uCtrl)
    {
        _uCtrl = uCtrl;
    }

    /// <summary>
    /// 매 프레임 업데이트 될 때 마다 작동
    /// </summary>
    /// <param name="dt"></param>
    virtual public void Update(float dt) { }

    /// <summary>
    /// 공격했을 때 작동
    /// </summary>
    virtual public void Attack(FieldObject enemyUnit) {  }

    /// <summary>
    /// 공격한 후 작동
    /// </summary>
    virtual public void Hit(ref float damage) {  }

    /// <summary>
    /// 데미지를 받았을 때 작동
    /// </summary>
    virtual public void Beaten(FieldObject enemyUnit) {  }
}
