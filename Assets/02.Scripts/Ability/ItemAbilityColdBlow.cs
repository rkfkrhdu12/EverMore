
public class ItemAbilityColdBlow : ItemAbility
{ // 차가운 일격
    FieldObject _target;

    readonly eCrowdControl _curType = eCrowdControl.Freezing;

    override public void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._isCrowdControls[(int)_curType])
        {
            damage += damage / Var[0];

            enemyUnit._abilNameList.Add(Name);
        }
    }

    void ExitCC(eCrowdControl type, FieldObject enemyUnit)
    {
        if(type == _curType)
        {
            enemyUnit._abilNameList.Remove(Name);
        }
    }
}
