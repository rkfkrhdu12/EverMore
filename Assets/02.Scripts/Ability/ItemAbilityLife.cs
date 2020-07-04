
public class ItemAbilityLife : ItemAbility
{ // 생명
    override public void Awake(UnitStatus us)
    {
        base.Awake(us);

        _us._curhealth += _us._curhealth / Var[0];
    }
}
