using UnityEngine;

using UnityEngine.UI;

public class Tower : FieldObject
{
    public Image _healthBar;

    public void Awake()
    {
        _canvasRectTrs = _canvas.GetComponent<RectTransform>();
        _hpCamera = _canvas.worldCamera;
    }

    public override void DamageReceive(float damage)
    {
        _curHp -= damage;

        _healthBar.fillAmount = RemainHealth;

        if (_curHp <= 0)
            _isDead = true;


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
