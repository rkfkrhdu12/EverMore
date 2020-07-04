public class Castle : FieldObject
{
    override protected void Awake()
    {
        base.Awake();

        _curHp = _maxHp = 100;
    }

    public override void DamageReceive(float damage, FieldObject receiveObject)
    {
        _curHp -= damage;

        if (_curHp <= 0)
            _isDead = true;
    }
}
