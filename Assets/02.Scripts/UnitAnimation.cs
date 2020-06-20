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

    private void OnEnable()
    {
        if(_unitCtrl == null)  { _unitCtrl = transform.parent.GetComponent<UnitController>(); LogMassage.Log("UnitAnimation : UnitCtrl is NULL"); } 
        if(_ani == null)       { _ani = GetComponent<Animator>();                             LogMassage.Log("UnitAnimation : Ani is NULL"); }

        _ani.SetFloat(_idAttackSpd, _unitCtrl._attackSpeed);
        _ani.SetBool(_idAttack, false);

        UnitAnimationManager.Update(_unitCtrl._status._equipedItems[2], _unitCtrl._status._equipedItems[3], _ani);
    }

    public void Update(UnitController.eAni curState)
    {
        UnitController.eAni prevState = _unitCtrl.CurState;
        if (prevState == curState) { return; }

        switch (prevState)
        {
            case UnitController.eAni.MOVE:    _ani.SetFloat(_idMove, 0.0f); break;
            case UnitController.eAni.ATTACK:  _ani.SetBool(_idAttack, false); break;
        }

        switch (curState)
        {
            case UnitController.eAni.IDLE:    _ani.SetFloat(_idMove, 0.0f); _ani.SetBool(_idAttack, false); break;
            case UnitController.eAni.MOVE:    _ani.SetFloat(_idMove, 1.0f);  break;
            case UnitController.eAni.ATTACK:  _ani.SetBool(_idAttack, true); break;
        }
    }

    public void OnEffect()
    {
        _unitCtrl.OnEffect();
    }

    public void AttackRight()
    {
        _unitCtrl.AttackRight();
    }

    public void AttackLeft()
    {
        _unitCtrl.AttackLeft();
    }
}
