﻿
public class ItemAbilityVanguard : ItemAbility
{ // 선봉
    public bool _isActive = false;

    int _count = 1;
    public override void Update(float dt)
    {
        if(Time > 0)
        {
            _time -= dt;

            if(Time <= 0)
            {
                _us._curhealth += Var[0];

                _uCtrl._curStateNum.Remove(5);
            }
        }

        if (_isActive && _count <= Var[1])
        {
            if(_time <= 0)
            {
                ++_count;

                _uCtrl._curStateNum.Add(5);
                _time = 1.0f;
            }
        }
        else if (_us.Health <= _condition)
        {
            _isActive = true;
        }
    }
}
