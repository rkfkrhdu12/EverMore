using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityFrostyFeet : ItemAbility
{
    bool _isUpdate = false;

    float _defaultAtkSpd  = -1;      
    float _defaultMoveSpd = -1;
    UnitController _curTarget;

    public override void Hit(FieldObject enemyUnit)
    {
        _curTarget = (UnitController)enemyUnit;
        if (_curTarget == null) { return; }

        if (!_isUpdate)
        {
            _isUpdate = true;

            _defaultAtkSpd = _curTarget._status._attackSpeed;
            _defaultMoveSpd = _curTarget._status._moveSpeed;

            _curTarget._status._attackSpeed = _defaultAtkSpd / _variables[0];
            _curTarget._status._moveSpeed = _defaultMoveSpd / _variables[1];
        }

        _time = _timeInterval;
    }

    public override void TimeOver()
    {
        _isUpdate = false;

        _curTarget._status._attackSpeed = _defaultAtkSpd;
        _curTarget._status._moveSpeed = _defaultMoveSpd;
    }
}

