using UnityEngine;

using UnityEngine.UI;

public class Tower : FieldObject
{
    public Image _healthBar;

    public SpawnManager _spawnMgr;

    public GameObject _brokenObject;

    void Awake()
    {
        _team = _spawnMgr._isPlayer ? eTeam.PLAYER : eTeam.ENEMY;
        _canvasRectTrs = _canvas.GetComponent<RectTransform>();
        _hpCamera = _canvas.worldCamera;
    }

    public override void DamageReceive(float damage, FieldObject receiveObject)
    {
        _curHp -= damage;

        _healthBar.fillAmount = RemainHealth;

        if (_curHp <= 0)
        {
            _isDead = true;
            _brokenObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public Canvas _canvas;

    private RectTransform _canvasRectTrs;
    private Camera _hpCamera;

    public int x = 3;
    public int y = 80;
    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTrs, screenPos, _hpCamera, out localPos);

        localPos.x += x;
        localPos.y += y;

        _healthBar.transform.parent.localPosition = localPos;
    }
}
