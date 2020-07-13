
public class ItemAbilityParvenu : ItemAbility
{ // 졸부
    override public void Awake(UnitStatus us)
    {
        base.Awake(us);

        _us._cost -= (int)(_us._cost / Var[0]);
    }

    public override void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

    }
}