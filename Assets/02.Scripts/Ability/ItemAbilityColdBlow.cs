
public class ItemAbilityColdBlow : ItemAbility
{ // 차가운 일격
    FieldObject _target;

    readonly eCrowdControl _curType = eCrowdControl.Freezing;

    override public void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._isCrowdControls[(int)_curType])
        {
            damage += damage / Var[0];
        }
    }
}
