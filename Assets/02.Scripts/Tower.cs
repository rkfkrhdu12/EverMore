
public class Tower : FieldObject
{


    public override void DamageReceive(float damage)
    {
        _curHp -= damage;

        if (_curHp <= 0)
            _isDead = true;
    }
}
