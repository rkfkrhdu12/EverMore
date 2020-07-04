
public class ItemAbilityDefenceSpiral : ItemAbility
{ // 파괴
    readonly eCrowdControl _curType = eCrowdControl.DefensiveSpiral;

    public override void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }


        if (!enemyUnit._isCrowdControls[(int)_curType])
        {
            enemyUnit.DefencePower -= enemyUnit.DefaultDefencePower / Var[0];
        }

        enemyUnit.CrowdControl(_curType, Time);
        enemyUnit.ExitCCFunction += ExitCC;
    }

    void ExitCC(eCrowdControl type, FieldObject enemyUnit)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (type == _curType)
        {
            enemyUnit.DefencePower += enemyUnit.DefaultDefencePower / Var[0];
        }

        enemyUnit.ExitCCFunction -= ExitCC;
    }
}
