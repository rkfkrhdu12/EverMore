public class ItemAbilitySharpness : ItemAbility
{
    public override void AttackStart(UnitStatus us, UnitStatus[] enemyUs)
    {
    }

    public override void StartSpawn(UnitStatus us)
    {
    }

    public override void UpdateStatus(UnitStatus us)
    {
        us._minAttackDamages[0] *= us._minAttackDamages[0] / _variables[0];
        us._minAttackDamages[1] *= us._minAttackDamages[1] / _variables[0];

        us._maxAttackDamages[0] *= us._maxAttackDamages[0] / _variables[1];
        us._maxAttackDamages[1] *= us._maxAttackDamages[1] / _variables[1];
    }
}
