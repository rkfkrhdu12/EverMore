﻿
public class ItemAbilityQuick : ItemAbility
{ // 신속
    override public void Awake(UnitStatus us)
    {
        base.Awake(us);

        _us._moveSpeed += _us._moveSpeed / Var[0];
    }

    public override void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

        _uCtrl._curStateNum.Add(9);
    }
}
