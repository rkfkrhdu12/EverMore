
public class ItemAbilityHotBlow : ItemAbility
{ // 뜨거운 일격
    FieldObject _target;

    readonly eCrowdControl _curType = eCrowdControl.Burn;

    override public void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._isCrowdControls[(int)_curType])
        {
            damage += damage / Var[0];
        }
    }
}
