
using System.Collections;

public class ItemAbilitySharpness : ItemAbility
{ // 날카로움 1 +
    override public void Hit(FieldObject enemyUnit) { LogMessage.Log("Hit"); }
    override public void Beaten(FieldObject enemyUnit) { LogMessage.Log("Beaten"); }

    override public void Init(UnitController unit)
    {
        LogMessage.Log("UpdateStatus");

        base.Init(unit);

        UnitStatus us = unit._status;

        us._minAttackDamages[0] *= us._minAttackDamages[0] / _variables[0];
        us._minAttackDamages[1] *= us._minAttackDamages[1] / _variables[0];

        us._maxAttackDamages[0] *= us._maxAttackDamages[0] / _variables[1];
        us._maxAttackDamages[1] *= us._maxAttackDamages[1] / _variables[1];

        unit._status = us;
    }
}
