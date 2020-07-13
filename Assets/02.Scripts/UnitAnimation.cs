using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private static readonly int _idAttack = Animator.StringToHash("Attack");
    private static readonly int _idAttackSpd = Animator.StringToHash("AttackSpeed");
    private static readonly int _idMove = Animator.StringToHash("Move");

    [SerializeField]
    private Animator _ani = null;

    [SerializeField]
    private UnitController _unitCtrl = null;

    public bool _isCustom = false;

    private void OnEnable()
    {
        if (_isCustom) { return; }

        if(_unitCtrl == null)  { _unitCtrl = transform.parent.GetComponent<UnitController>(); LogMessage.Log("UnitAnimation : UnitCtrl is NULL"); } 
        if(_ani == null)       { _ani = GetComponent<Animator>();                             LogMessage.Log("UnitAnimation : Ani is NULL"); }

        _ani.SetFloat(_idAttackSpd, _unitCtrl.AttackSpeed);
        _ani.SetBool(_idAttack, false);

        if (_unitCtrl._status._equipedItems == null) { LogMessage.Log("UNitCtrl Status is null Error"); return; }

        UnitAnimationManager.Update(_unitCtrl._status._equipedItems[2], _unitCtrl._status._equipedItems[3], _ani);
    }

    public void UpdateAni(eAni curState)
    {
        eAni prevState = _unitCtrl.CurState;
        if (prevState == curState) { return; }

        switch (prevState)
        {
            case eAni.MOVE:    _ani.SetFloat(_idMove, 0.0f); break;
            case eAni.ATTACK:  _ani.SetBool(_idAttack, false); break;
        }

        switch (curState)
        {
            case eAni.IDLE:    _ani.SetFloat(_idMove, 0.0f); _ani.SetBool(_idAttack, false); break;
            case eAni.MOVE:    _ani.SetFloat(_idMove, 1.0f);  break;
            case eAni.ATTACK:  _ani.SetBool(_idAttack, true); break;
        }
    }

    public void OnEffect()
    {
        if (_unitCtrl == null) { return; }

        _unitCtrl.OnEffect();
    }

    public void AttackRight()
    {
        if (_unitCtrl == null) { return; }

        _unitCtrl.AttackRight();
    }

    public void AttackLeft()
    {
        if (_unitCtrl == null) { return; }

        _unitCtrl.AttackLeft();
    }
}
