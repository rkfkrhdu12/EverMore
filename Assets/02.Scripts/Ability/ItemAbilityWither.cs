
public class ItemAbilityWither : ItemAbility
{ // 쇄약
    readonly eCrowdControl _curType = eCrowdControl.Wither;

    public override void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (!enemyUnit._isCrowdControls[(int)_curType])
        {
            _uCtrl._curStateNum.Add(10);

            enemyUnit.AttackSpeed   -= enemyUnit.DefaultAttackSpeed / Var[0];
            enemyUnit.MoveSpeed     -= enemyUnit.DefaultMoveSpeed / Var[1];
        }

        enemyUnit.CrowdControl(_curType, Time);
        enemyUnit.ExitCCFunction += ExitCC;
    }

    void ExitCC(eCrowdControl type, FieldObject enemyUnit)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (type == _curType)
        {
            _uCtrl._curStateNum.Remove(10);

            enemyUnit.AttackSpeed   += enemyUnit.DefaultAttackSpeed / Var[0];
            enemyUnit.MoveSpeed     += enemyUnit.DefaultMoveSpeed / Var[1];
        }

        enemyUnit.ExitCCFunction -= ExitCC;
    }
}
