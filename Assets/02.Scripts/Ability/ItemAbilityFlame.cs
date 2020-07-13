using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityFlame : ItemAbility
{ // 불길
    readonly eCrowdControl _curType = eCrowdControl.Burn;

    public override void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (!enemyUnit._isCrowdControls[(int)_curType])
        {
            _uCtrl._abilNameList.Add(Name);

        }

        enemyUnit.CrowdControl(_curType, 1.0f);
        enemyUnit.ExitCCFunction += ExitCC;
    }

    int ccCount = 1;
    void ExitCC(eCrowdControl type, FieldObject enemyUnit)
    {
        if (enemyUnit._team == _uCtrl._team) { return; }

        if (type == _curType)
        {
            _uCtrl._abilNameList.Remove(Name);

            enemyUnit.DamageDot(Var[0]);
        }

        if (ccCount >= Time)
        {
            _uCtrl._abilNameList.Remove(Name);
            enemyUnit.ExitCCFunction -= ExitCC;
        }
    }

}
