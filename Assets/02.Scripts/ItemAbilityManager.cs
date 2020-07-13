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

        abilityList.Add(new ItemAbilitySharpness());            // 날카로움
        abilityList.Add(new ItemAbilitySharpness());            // 날카로움
        abilityList.Add(new ItemAbilityFrostyFeet());           // 서릿발
        abilityList.Add(new ItemAbilityChill());                // 한기
        abilityList.Add(new ItemAbilityInspire());              // 격려
        abilityList.Add(new ItemAbilityVanguard());             // 선봉
        abilityList.Add(new ItemAbilityParvenu());              // 졸부
        abilityList.Add(new ItemAbilityWisdom());               // 지혜
        abilityList.Add(new ItemAbilityLife());                 // 생명
        abilityList.Add(new ItemAbilityQuick());                // 신속
        abilityList.Add(new ItemAbilityWither());               // 쇄약

        abilityList.Add(new ItemAbilityFlame());                // 불길
        abilityList.Add(new ItemAbility());                     // 독약
        abilityList.Add(new ItemAbility());                     // 개전
        abilityList.Add(new ItemAbility());                     // 원시
        abilityList.Add(new ItemAbility());                     // 광기
        abilityList.Add(new ItemAbilityIncreaseMorale());       // 사기진작

        abilityList.Add(new ItemAbilityLifesteal());            // 흡혈

        abilityList.Add(new ItemAbility());                     // 단말마 X
        abilityList.Add(new ItemAbility());                     // 돌파
        abilityList.Add(new ItemAbility());                     // 약탈

        abilityList.Add(new ItemAbilityColdBlow());             // 차가운일격
        abilityList.Add(new ItemAbilityHotBlow());              // 뜨거운일격
        abilityList.Add(new ItemAbilityMeanBlow());             // 비열한일격

        abilityList.Add(new ItemAbility());                     // 은신
        abilityList.Add(new ItemAbility());                     // 피로

        abilityList.Add(new ItemAbilityDefenceSpiral());        // 파괴
        abilityList.Add(new ItemAbilityDefencePenetrate());     // 관통
        abilityList.Add(new ItemAbilityDemolish());             // 철거

        abilityList.Add(new ItemAbility());                     // 반격

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

            abilityList[index]._info = splitDatas[7];

            if (!UnitAbilityIconManager._isAddNameEnd)
                UnitAbilityIconManager.AddName(abilityList[index].Name);

            _abilityList.Add(int.Parse(splitDatas[0]), abilityList[index++]);
        }

        UnitAbilityIconManager._isAddNameEnd = true;
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
    public string _info;                                public string Info => _info;

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
    virtual public void Attack(FieldObject enemyUnit, ref float damage) {  }

    /// <summary>
    /// 공격한 후 작동
    /// </summary>
    virtual public void Hit(ref float damage) {  }

    /// <summary>
    /// 데미지를 받았을 때 작동
    /// </summary>
    virtual public void Beaten(FieldObject enemyUnit) {  }
}
