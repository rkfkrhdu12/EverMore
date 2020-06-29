
using System.Collections;

public class ItemAbilitySharpness : ItemAbility
{
    public override IEnumerator UpdateAttack(UnitController unit, FieldObject enemyUnits)
    {
        yield return null;
    }

    public override void StartSpawn(ref UnitController unit)
    {
    }

    public override void UpdateStatus(ref UnitController unit)
    {
        UnitStatus us = unit._status;

        us._minAttackDamages[0] *= us._minAttackDamages[0] / _variables[0];
        us._minAttackDamages[1] *= us._minAttackDamages[1] / _variables[0];

        us._maxAttackDamages[0] *= us._maxAttackDamages[0] / _variables[1];
        us._maxAttackDamages[1] *= us._maxAttackDamages[1] / _variables[1];

        unit._status = us;
    }
}
