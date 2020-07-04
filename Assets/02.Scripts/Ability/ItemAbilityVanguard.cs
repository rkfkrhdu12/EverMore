using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityVanguard : ItemAbility
{ // 선봉

    public bool _isActive = false;

    int _count = 1;
    public override void Update(float dt)
    {
        base.Update(dt);

        if (_isActive && _count <= Var[1])
        {
            if(_time > 0)
            {
                ++_count;
                _time = 1.0f;
            }
        }
        else if (_us.Health <= _condition)
        {
            _isActive = true;
        }
    }

    public override void TimeOver()
    {
        _us._curhealth += Var[0];
    }
}
