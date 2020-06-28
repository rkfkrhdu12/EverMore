using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemAbilityManager
{
    Dictionary<int, ItemAbility> _abilityList = new Dictionary<int, ItemAbility>();

    public ItemAbility GetAbility(int Code)
    {
        if (!_abilityList.ContainsKey(Code)) { return null; }

        return _abilityList[Code];
    }

    private void Init()
    {
        var abilityDatas = CSVParser.Read("AbilityTable");

        List<ItemAbility> abilityList = new List<ItemAbility>();

        abilityList.Add(new ItemAbilitySharpness());
        abilityList.Add(new ItemAbilitySharpness());

        int index = 0;
        foreach (var splitDatas in abilityDatas.Select(t => t.Split(',')))
        {
            if(index >= abilityList.Count) { break; }

            abilityList[index]._name            = splitDatas[1];
            abilityList[index]._range           = float.Parse(splitDatas[2]);
            abilityList[index]._time            = int.Parse(splitDatas[3]);
            abilityList[index]._condition       = float.Parse(splitDatas[4]);

            float var;
            var = float.Parse(splitDatas[5]);
            if (var != 0)
                abilityList[index]._variables.Add(var);

            var = float.Parse(splitDatas[6]);
            if (var != 0)
                abilityList[index]._variables.Add(var);

            ++index;
        }
    }
}

public class ItemAbility
{
    public string _name;                    public string Name      => _name;
    public float _range;                    public float Range      => _range;
    public int _time;                       public int Time         => _time;

    public float _condition;                public float Condition  => _condition;

    public List<float> _variables;          public List<float> Var  => _variables;

    virtual public void UpdateStatus(UnitStatus us) { }
    virtual public void StartSpawn(UnitStatus us) { }
    virtual public void AttackStart(UnitStatus us, UnitStatus[] enemyUs) { }
}
