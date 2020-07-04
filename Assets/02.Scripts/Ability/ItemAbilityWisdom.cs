
public class ItemAbilityWisdom : ItemAbility
{ // 지혜
    override public void Awake(UnitStatus us)
    {
        base.Awake(us);

        _us._coolTime -= _us._coolTime / Var[0];
    }
}
