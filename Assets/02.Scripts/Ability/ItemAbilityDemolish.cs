
public class ItemAbilityDemolish : ItemAbility
{ // 철거

    override public void Attack(FieldObject enemyUnit, ref float damage)
    {
        Tower t = enemyUnit.gameObject.GetComponent<Tower>();

        if (t != null)
        {
            damage += damage / Var[0];
        }
    }
}
