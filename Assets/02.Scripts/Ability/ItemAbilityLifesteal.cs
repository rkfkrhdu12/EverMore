using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityLifesteal : ItemAbility
{
    public override void Attack(FieldObject enemyUnit)
    {
        HitTarget = enemyUnit;
    }

    FieldObject HitTarget;

    public override void Hit(ref float damage)
    {
        HitTarget.CurHealth
    }
}
