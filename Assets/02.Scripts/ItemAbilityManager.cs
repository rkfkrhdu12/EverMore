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
            float   .TryParse(splitDatas[3], out abilityList[index]._time);
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
    public float _time;                                 public float Time => _time;
    public float _condition;                            public float Condition => _condition;
    public List<float> _variables = new List<float>();  public List<float> Var => _variables;

    // Private

    protected UnitStatus _us;
    protected UnitController _uCtrl;
    protected float _timeInterval;

    // Function

    /// <summary>
    /// 스폰되기 전에 작동
    /// </summary>
    virtual public void Init(UnitStatus us)
    {
        _us = us;

        _timeInterval = Time;
    }

    /// <summary>
    /// 다시 시작될 때 마다 작동
    /// </summary>
    /// <param name="uCtrl"></param>
    virtual public void Enable(UnitController uCtrl)
    {
        _uCtrl = uCtrl;
    }

    /// <summary>
    /// 매 프레임 업데이트 될 때 마다 작동
    /// </summary>
    /// <param name="dt"></param>
    virtual public void Update(float dt)
    {
        if (_time <= 0) { return; }

        _time -= dt;
        if (_time <= 0) { TimeOver(); }
    }

    /// <summary>
    /// 스폰되고 출발할때 작동
    /// </summary>
    virtual public void Awake() { }

    /// <summary>
    /// 공격했을 때 작동
    /// </summary>
    virtual public void Hit(FieldObject enemyUnit) {  }
    /// <summary>
    /// 데미지를 받았을 때 작동
    /// </summary>
    virtual public void Beaten(FieldObject enemyUnit) {  }

    virtual public void TimeOver() { }

}
