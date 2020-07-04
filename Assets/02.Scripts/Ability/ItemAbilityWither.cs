using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityWither : ItemAbility
{ // 쇄약
    eCrowdControl curCC = eCrowdControl.Wither;

    public override void Attack(FieldObject enemyUnit)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (!enemyUnit._isCrowdControls[(int)curCC])
        {
            enemyUnit.AttackSpeed   -= enemyUnit.DefaultAttackSpeed / Var[0];
            enemyUnit.MoveSpeed     -= enemyUnit.DefaultMoveSpeed / Var[1];
        }

        enemyUnit.CrowdControl(curCC, Time);
        enemyUnit.ExitCCFunction += ExitCC;
    }

    void ExitCC(eCrowdControl type, FieldObject enemyUnit)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (type == curCC)
        {
            enemyUnit.AttackSpeed   += enemyUnit.DefaultAttackSpeed / Var[0];
            enemyUnit.MoveSpeed     += enemyUnit.DefaultMoveSpeed / Var[1];
        }

        enemyUnit.ExitCCFunction -= ExitCC;
    }
}
