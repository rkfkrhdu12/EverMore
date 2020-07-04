
public class ItemAbilityBreakthrough : ItemAbility
{ // 돌파
    bool _isOn = false;

    override public void Update(float dt)
    {
        if(_uCtrl._isCrowdControls[(int)eCrowdControl.Freezing] && !_isOn)
        {
            _isOn = true;

            _uCtrl.AttackSpeed  += _uCtrl.AttackSpeed / Var[0];
            _uCtrl.MoveSpeed    += _uCtrl.MoveSpeed / Var[0];
        }
        else if (!_uCtrl._isCrowdControls[(int)eCrowdControl.Freezing] && _isOn)
        {
            _isOn = false;

            _uCtrl.AttackSpeed  -= _uCtrl.AttackSpeed / Var[0];
            _uCtrl.MoveSpeed    -= _uCtrl.MoveSpeed / Var[0];
        }
    }
}
