public class Castle : FieldObject
{
    private void Awake() =>
        _curHp = _maxHp = 100;

    public override void DamageReceive(float damage, FieldObject receiveObject)
    {
        _curHp -= damage;

        if (_curHp <= 0)
            _isDead = true;
    }
}
