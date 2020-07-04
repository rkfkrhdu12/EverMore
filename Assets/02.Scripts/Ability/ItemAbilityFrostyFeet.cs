﻿
using System.Collections;
using System.Collections.Generic;

public class ItemAbilityFrostyFeet : ItemAbility
{ // 서릿발
    eCrowdControl curCC = eCrowdControl.FrostyFeet;

    public override void Attack(FieldObject enemyUnit)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }


        if (!enemyUnit._isCrowdControls[(int)curCC])
        {
            enemyUnit.AttackSpeed   -= enemyUnit.DefaultAttackSpeed / Var[0];
            enemyUnit.MoveSpeed     -= enemyUnit.DefaultMoveSpeed / Var[0];
        }

        enemyUnit.CrowdControl(curCC, Time);
        enemyUnit.ExitCCFunction += ExitCC;
    }

    void ExitCC(eCrowdControl type, FieldObject enemyUnit)
    {
        if(enemyUnit._team == _uCtrl._team) { return; }

        if(type == curCC)
        {
            enemyUnit.AttackSpeed -= enemyUnit.DefaultAttackSpeed / Var[0];
            enemyUnit.MoveSpeed -= enemyUnit.DefaultMoveSpeed / Var[0];
        }

        enemyUnit.ExitCCFunction -= ExitCC;
    }
}

