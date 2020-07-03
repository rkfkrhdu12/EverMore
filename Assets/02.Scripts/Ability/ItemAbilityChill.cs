
using System.Collections.Generic;
using UnityEngine;

public class ItemAbilityChill : ItemAbility
{ // 한기

    List<FieldObject> _chillObjectList = new List<FieldObject>();

    public override void Update(float dt)
    {
        _chillObjectList = _uCtrl._eye._targets;

        for (int i = 0; i < _chillObjectList.Count; ++i)
        {
            Vector3 direction = _chillObjectList[i].transform.position - _uCtrl.transform.position;

            // 길이 체크  너무 멀진 않은가 ?
            if (direction.magnitude < _range)
            {
                // 이미 빙결상태거나 죽은 상대면 패스
                if (_chillObjectList[i]._crowdControls[(int)eCrowdControl.Freezing] || _chillObjectList[i].IsDead) return;

                _chillObjectList[i]._crowdControls[(int)eCrowdControl.Freezing] = true;


            }
        }
    }
}
