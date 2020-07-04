
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityChill : ItemAbility
{ // 한기

    List<FieldObject> _objectList = new List<FieldObject>();

    int curCCType = (int)eCrowdControl.Freezing;

    public override void Update(float dt)
    {
        for (int i = 0; i < _objectList.Count; ++i)
        {
            if (!_uCtrl._eye._enemyTargets.Contains(_objectList[i]))
            {
                _objectList[i]._isCrowdControls[curCCType] = false;

                _objectList[i].AttackSpeed += _objectList[i].AttackSpeed / Var[0];
            }
        }

        _objectList = _uCtrl._eye._enemyTargets;

        for (int i = 0; i < _objectList.Count; ++i)
        {
            Vector3 direction = _objectList[i].transform.position - _uCtrl.transform.position;

            // 길이 체크  너무 멀진 않은가 ?
            if (direction.magnitude < _range)
            {
                // 이미 빙결상태거나 죽은 상대면 패스
                if (_objectList[i]._isCrowdControls[curCCType] || _objectList[i].IsDead) return;

                _objectList[i]._isCrowdControls[curCCType] = true;

                _objectList[i].AttackSpeed -= _objectList[i].AttackSpeed / Var[0];
            }
        }
    }
}
