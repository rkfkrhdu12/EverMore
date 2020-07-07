
public class ItemAbilityLife : ItemAbility
{ // 생명
    override public void Awake(UnitStatus us)
    {
        base.Awake(us);

        _us._curhealth += _us._curhealth / Var[0];
    }

    public override void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

        _uCtrl._curStateNum.Add(8);
    }
}
