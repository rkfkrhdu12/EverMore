
public class ItemAbilityDefencePenetrate : ItemAbility
{ // 관통
    override public void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

        uCtrl._defenseCleavage = Var[0];
    }
}

