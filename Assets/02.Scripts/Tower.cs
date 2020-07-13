using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Tower : FieldObject
{
    public Image _healthBar;

    public SpawnManager _spawnMgr;

    public GameObject _brokenObject;

    public bool _isBottomTower = false;

    List<UnitController> _targetList = new List<UnitController>();

    UnitController CurTarget { get { if (_targetList.Count != 0) return _targetList[0]; else return null; } }

    public Canvas _canvas;

    private RectTransform _canvasRectTrs;
    private Camera _hpCamera;

    readonly float _attackInterval = 1.0f;
    float _attackTime = 1.0f;


    public override void DamageReceive(float damage, FieldObject receiveObject)
    {
        _curHp -= damage;

        _healthBar.fillAmount = RemainHealth;

        if (_curHp <= 0)
        {
            _isDead = true;

            _brokenObject.SetActive(true);
            _healthBar.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);

            if (_spawnMgr._isPlayer)
                if (_isBottomTower)
                    _spawnMgr.DestroyBottomTower();
                else
                    _spawnMgr.DestroyTopTower();
        }
    }


    void Awake()
    {
        _team = _spawnMgr._isPlayer ? eTeam.PLAYER : eTeam.ENEMY;
        _canvasRectTrs = _canvas.GetComponent<RectTransform>();
        _hpCamera = _canvas.worldCamera;
    }

    override protected void FixedUpdate()
    {
        if (CurTarget == null) { return; }
        UnitController updateTarget = CurTarget;

        _attackTime -= Time.deltaTime;
        if (_attackTime <= 0)
        {
            _attackTime = _attackInterval;

            updateTarget.DamageReceive(10, this);
            if(updateTarget.CurHealth <= 0) { _targetList.Remove(updateTarget); }
        }
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTrs, screenPos, _hpCamera, out localPos);

        localPos.x += 3;
        localPos.y += 120;

        _healthBar.transform.parent.localPosition = localPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Unit") || other.isTrigger) { return; }
        
        UnitController uc = other.GetComponent<UnitController>();

        if (uc == null || uc._team == _team) { return; }

        _targetList.Add(uc);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Unit") || other.isTrigger) { return; }

        UnitController uc = other.GetComponent<UnitController>();
        if (uc == null) { return; }
        
        _targetList.Remove(uc);
    }
}
