
public class ItemAbilitySharpness : ItemAbility
{ // 날카로움 1 +
    override public void Init(UnitStatus unit)
    {
        base.Init(unit);
    }

    public override void Awake()
    {
        UnitStatus us = _us;

        us._minAttackDamages[0] += us._minAttackDamages[0] / _variables[0];
        us._minAttackDamages[1] += us._minAttackDamages[1] / _variables[0];

        us._maxAttackDamages[0] += us._maxAttackDamages[0] / _variables[1];
        us._maxAttackDamages[1] += us._maxAttackDamages[1] / _variables[1];
    }
}
