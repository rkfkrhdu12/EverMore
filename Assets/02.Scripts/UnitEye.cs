using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;

public class UnitEye : MonoBehaviour
{
    [SerializeField]
    private SphereCollider _collider = null;

    [SerializeField]
    private UnitController _unitCtrl = null;

    private float _attackRange = -1;
    private float _attackAngle = 180f;

    public bool _isEnemy = true;

    //공격 타겟에대한 큐
    public List<FieldObject> _allTargets    = new List<FieldObject>();
    public List<FieldObject> _enemyTargets  = new List<FieldObject>();
    public List<FieldObject> _friendTargets = new List<FieldObject>();

    private float _minRange = 6f;

    /// <summary>
    /// 충돌하지 않고 있으면 Null
    /// </summary>
    public FieldObject CurTarget
    {
        get
        {
            if (_enemyTargets.Count == 0)
            {
                return null;
            }

            return _enemyTargets[0];
        }
    }

    public void UpdateTarget()
    {
        if (_enemyTargets.Count <= 0) { return; }

        if (CurTarget.CurHealth <= 0) 
        {
            _allTargets.Remove(_enemyTargets[0]);
            _enemyTargets.Remove(_enemyTargets[0]);

            // UnitCtrl 업데이트
            _unitCtrl.UpdateTarget();
        }
    }

    #region Private Function

    #endregion

    #region Monobehaviour Function

    private void OnEnable()
    {
        // Inspector 에서 드래그드롭 해줘야 할 오브젝트들
        if (!_collider) { _collider = GetComponent<SphereCollider>(); LogMessage.Log("UnitEye : Collider is NULL"); }
        if (!_unitCtrl) { _unitCtrl = transform.parent.GetComponent<UnitController>(); LogMessage.Log("UnitEye : UnitCtrl is NULL"); }

        if(_unitCtrl._isTest) { return; }

        float range = Mathf.Max(_unitCtrl._status._attackRange, _minRange);

        _attackRange = range;
        _collider.radius = range * 1.25f;

        StartCoroutine(SortTarget());
    }

    WaitForSeconds _sortTime = new WaitForSeconds(.5f);
    IEnumerator SortTarget()
    {
        while (gameObject.activeSelf)
        {
            for (int i = 0; i < _allTargets.Count; ++i)
            {
                FieldObject curTarget = _allTargets[i];

                if (curTarget._team == _unitCtrl._team)
                { // 아군일때
                    if (_friendTargets.Contains(curTarget)) { continue; }

                    _friendTargets.Add(curTarget);
                }
                else
                { // 적군일때
                    if (_enemyTargets.Contains(curTarget)) { continue; }

                    // 범위 안에 있는지 체크
                    float dotValue = Mathf.Cos(Mathf.Deg2Rad * (_attackAngle / 2));
                    Vector3 direction = curTarget.transform.position - transform.position;

                    // 길이 체크  너무 멀진 않은가 ?
                    if (direction.magnitude < _attackRange)
                    {
                        // 각도 체크  정해진 각도를 넘진 않았는가 ?
                        if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
                        {
                            // 이미 타겟들에 있으면
                            if (_enemyTargets.Contains(curTarget) || curTarget.IsDead) continue;

                            // 타겟들 목록에 Add
                            _enemyTargets.Add(curTarget);

                            // UnitCtrl 업데이트
                            _unitCtrl.UpdateTarget();
                        }
                    }
                }
            }

            yield return _sortTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 아직 Init되지 않았거나, Unit이 아니거나 Collider가 몸이 아니거나, unitCtrl 이 Null일때
        if ((other.isTrigger) || _unitCtrl == null) { return; }

        // 유닛들대상
        {
            var target = other.GetComponent<FieldObject>();

            // target이 UnitCtrl 일때
            if (target != null)
            {
                _allTargets.Add(target);

                if (target._team == _unitCtrl._team) { return; }

                
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.GetComponent<FieldObject>();

        // FieldObject 가 있으면
        if (target != null)
        {
            if (_allTargets.Contains(target)) { return; }

            _allTargets.Remove(target);

            if (_friendTargets.Contains(target))
            {
                _friendTargets.Remove(target);
            }
            else if (_enemyTargets.Contains(target))
            {
                _enemyTargets.Remove(target);

                _unitCtrl.UpdateTarget();
            }
        }
    } 
    #endregion
}