
public class ItemAbilityMeanBlow : ItemAbility
{ // 비열한 일격
    FieldObject _target;

    readonly eCrowdControl _curType = eCrowdControl.Poisoned;

    override public void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._isCrowdControls[(int)_curType])
        {
            damage += damage / Var[0];
        }
    }
}
