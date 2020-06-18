using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnitEye : MonoBehaviour
{
    [SerializeField]
    private BoxCollider _collider = null;

    private UnitController _unitCtrl;

    private float _attackRange = -1;
    private float _attackAngle = 75f;

    private Vector3 _collideSize = new Vector3(3, 1, 1);
    private Vector3 _collideCenter = new Vector3(0, 1, .5f);

    private bool _isCollision = false;
    private bool _isInit = false;
    public bool _isEnemy = false;

    //공격 타겟에대한 큐
    public List<FieldObject> _targets = new List<FieldObject>();

    public void Init(in UnitController unitCtrl)
    {
        _unitCtrl = unitCtrl;
        _attackRange = _unitCtrl._status._attackRange;

        if (!_collider) _collider = GetComponent<BoxCollider>();
        
        _collideSize.z   = _attackRange;
        _collideCenter.z = _attackRange * .5f;

        _collider.size   = _collideSize;
        _collider.center = _collideCenter;

        _isInit = true;
    }

    /// <summary>
    /// 충돌하지 않고 있으면 Null
    /// </summary>
    public FieldObject CurTarget
    {
        get
        {
            if (!_isCollision)
                return null;
            
            return _targets[0];
        }
    }

    // Test
    private void OnDrawGizmos()
    {
        Color _blue = new Color(0f, 0f, 1f, 0.2f);
        Color _red = new Color(1f, 0f, 0f, 0.2f);

        Handles.color = _isCollision ? _red : _blue;
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, _attackAngle / 2, _attackRange * 2);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -_attackAngle / 2, _attackRange * 2);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!_isInit || (other.CompareTag("Unit") && other.isTrigger) || _unitCtrl == null) { return; }

        // 유닛들대상
        {
            var target = other.GetComponent<UnitController>();

            if (target != null)
            {
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (_attackAngle / 2));
                Vector3 direction = target.transform.position - transform.position;

                if (direction.magnitude < _attackRange * 2)
                {
                    if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
                    {
                        if (_targets.Contains(target)) return;

                        // 충돌 범위에 충돌함 !
                        _isCollision = true;
                        _isEnemy = target._team != _unitCtrl._team;

                        _targets.Add(target);
                    }
                    else
                        _isCollision = false;
                }
                else
                    _isCollision = false;

                return;
            }
        }

        // 포탑, 성채대상
        {
            var target = other.GetComponent<FieldObject>();


        }
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.GetComponent<FieldObject>();

        if (_targets.Contains(target))
        {
            _targets.Remove(target);
        }
    }
}


//private void OnTriggerEnter(Collider other)
//{
//    //닿은 생대가 유닛이 아니라면 : 아래 코드 구문 실행 X
//    if (!other.CompareTag("Unit") || other.isTrigger)
//        return;

//    //other의 필드 관점의 데이터를 가져옴.
//    var target = other.GetComponent<FieldObject>();

//    //공격할 타겟 큐에 해당 타겟이 있거나, 타겟의 팀이 우리 팀이라면,
//    if (_attackTargets.Contains(target) || target._team == _team)
//        return;

//    // if(_team == eTeam.PLAYER) { Debug.Log("Enemy Check !"); }

//    //공격 타겟에 해당 타겟을 넣어줍니다.
//    _attackTargets.Enqueue(target);
//}