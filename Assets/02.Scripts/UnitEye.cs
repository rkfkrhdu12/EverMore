using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InspectorGadgets.Editor;

public class UnitEye : MonoBehaviour
{
    [SerializeField]
    private SphereCollider _collider = null;

    private UnitController _unitCtrl;

    private float _attackRange = -1;
    private float _attackAngle = 180f;

    private bool _isCollision = false;
    private bool _isInit = false;
    public bool _isEnemy = false;

    //공격 타겟에대한 큐
    public List<FieldObject> _targets = new List<FieldObject>();

    private float _minRange = 3.2f;
    private float _maxRange = 6.0f;

    public void Init(in UnitController unitCtrl)
    {
        // Inspector 에서 드래그드롭 해줘야 할 오브젝트들
        if (!_collider) { _collider = GetComponent<SphereCollider>(); Debug.Log("UnitEye Collider is Null"); }

        // 나머지 데이터들 Init
        _unitCtrl = unitCtrl;

        float range = Mathf.Clamp(_unitCtrl._status._attackRange, _minRange, _maxRange);

        _attackRange = range * 2;
        _collider.radius = range * 2.5f;

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
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, _attackAngle / 2, _attackRange);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -_attackAngle / 2, _attackRange);
    }

    private void OnTriggerStay(Collider other)
    {
        // 아직 Init되지 않았거나, Unit이 아니거나 Collider가 몸이 아니거나, unitCtrl 이 Null일때
        if (!_isInit || (other.CompareTag("Unit") && other.isTrigger) || _unitCtrl == null)  { return; }

        // 유닛들대상
        {
            var target = other.GetComponent<UnitController>();

            // target이 UnitCtrl 일때
            if (target != null)
            {
                // 범위 안에 있는지 체크
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (_attackAngle / 2));
                Vector3 direction = target.transform.position - transform.position;

                // 길이 체크  너무 멀진 않은가 ?
                if (direction.magnitude < _attackRange)
                {
                    // 각도 체크  정해진 각도를 넘진 않았는가 ?
                    if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
                    {
                        // 이미 타겟들에 있으면
                        if (_targets.Contains(target)) return;

                        // 충돌 범위에 충돌함 !
                        _isCollision = true;
                        // 아군인지 적군인지 체크
                        _isEnemy = target._team != _unitCtrl._team;

                        // 타겟들 목록에 Add
                        _targets.Add(target);

                        // UnitCtrl 업데이트
                        _unitCtrl.UpdateTarget();
                    }
                    else // 충돌 안함
                        _isCollision = false;
                }
                else // 충돌 안함
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

        // FieldObject 가 있으면
        if (target != null)
        {
            // 현재 타겟들에 존재하고 아군이면
            if (_targets.Contains(target) || target._team == _unitCtrl._team)
            {
                // 뺀다.
                _targets.Remove(target);
            }
        }
    }
}