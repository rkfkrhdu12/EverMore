
public class ItemAbilitySharpness : ItemAbility
{ // 날카로움 1,+
    override public void Awake(UnitStatus unit)
    {
        base.Awake(unit);

        _us._minAttackDamages[0] += _us._minAttackDamages[0] / _variables[0];
        _us._minAttackDamages[1] += _us._minAttackDamages[1] / _variables[0];

        _us._maxAttackDamages[0] += _us._maxAttackDamages[0] / _variables[1];
        _us._maxAttackDamages[1] += _us._maxAttackDamages[1] / _variables[1];
    }

    override public void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

        _uCtrl._curStateNum.Add(1);
    }

}
