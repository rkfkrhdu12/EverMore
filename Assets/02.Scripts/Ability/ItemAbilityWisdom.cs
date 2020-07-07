
public class ItemAbilityWisdom : ItemAbility
{ // 지혜
    override public void Awake(UnitStatus us)
    {
        base.Awake(us);

    }

    public override void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

        _uCtrl._curStateNum.Add(7);
    }
}
