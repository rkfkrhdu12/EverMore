using UnityEngine;

public class ItemAbilityLifesteal : ItemAbility
{ // 흡혈
    public override void Attack(FieldObject enemyUnit, ref float damage)
    {
        if (_target != enemyUnit)
            _target = enemyUnit;
    }

    FieldObject _target;

    public override void Hit(ref float damage)
    {
        float overDamage = Mathf.Min(_target.CurHealth - damage, 0);
        float heal = damage - overDamage;

        _uCtrl.Heal(heal);
    }
}
