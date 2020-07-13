using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemAbilityIncreaseMorale : ItemAbility
{ // 사기진작
    List<FieldObject> _objectList = new List<FieldObject>();

    readonly int _curType = (int)eCrowdControl.IncreaseMorale;

    public override void Start(UnitController uCtrl)
    {
        base.Start(uCtrl);

        // Yellow
        _uCtrl._abilEffectObject.transform.GetChild(2).gameObject.SetActive(true);
    }

    public override void Update(float dt)
    {
        for (int i = 0; i < _objectList.Count; ++i)
        {
            if (!_uCtrl._eye._friendTargets.Contains(_objectList[i]))
            {
                _objectList[i]._isCrowdControls[_curType] = false;

                _objectList[i]._abilNameList.Remove(Name);

                _objectList[i].GetComponent<NavMeshAgent>().speed -= _objectList[i].MoveSpeed / Var[0];
            }
        }

        _objectList = _uCtrl._eye._friendTargets;

        for (int i = 0; i < _objectList.Count; ++i)
        {
            Vector3 direction = _objectList[i].transform.position - _uCtrl.transform.position;

            // 길이 체크  너무 멀진 않은가 ?
            if (direction.magnitude < _range)
            {
                // 이미 빙결상태거나 죽은 상대면 패스
                if (_objectList[i]._isCrowdControls[_curType] || _objectList[i].IsDead) return;

                _objectList[i]._isCrowdControls[_curType] = true;

                _objectList[i]._abilNameList.Add(Name);

                _objectList[i].GetComponent<NavMeshAgent>().speed += _objectList[i].MoveSpeed / Var[0];
            }
        }
    }
}
