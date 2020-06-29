using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityFrostyFeet : ItemAbility
{
    bool _isUpdate = false;
    public override IEnumerator UpdateAttack(UnitController unit, FieldObject enemyUnits)
    {
        UnitController curTarget = (UnitController)enemyUnits;
        if (curTarget == null || _isUpdate) { yield return null; }

        _isUpdate = true;

        float defaultAtkSpd = curTarget._status._attackSpeed;
        float defaultMoveSpd = curTarget._status._moveSpeed;

        curTarget._status._attackSpeed = defaultAtkSpd / _variables[0];
        curTarget._status._moveSpeed = defaultMoveSpd / _variables[0];

        yield return WaitTime;

        curTarget._status._attackSpeed = defaultAtkSpd;
        curTarget._status._moveSpeed = defaultMoveSpd;

        _isUpdate = false;
        yield return null;
    }

    public override void StartSpawn(ref UnitController unit)
    {
    }

    public override void UpdateStatus(ref UnitController unit)
    {
    }
}

