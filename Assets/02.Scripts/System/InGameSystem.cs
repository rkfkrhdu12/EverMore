using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSystem : MonoBehaviour
{
    #region Private Variable
    [SerializeField]
    private FieldObject _player1Base = null;

    [SerializeField]
    private Image _player1Bar = null;

    [SerializeField]
    private FieldObject _player2Base = null;

    [SerializeField]
    private Image _player2Bar = null;

    [SerializeField]
    private TextMeshProUGUI _timerText = null;
    private float _timerUITime;

    [SerializeField]
    private Image _goldImage = null;

    private float _curGold;
    private const float _maxGold = 500;
    private const float _goldPerSecond = 10;

    [SerializeField]
    private Image _manaImage = null;

    private float _curMana;
    private const float _maxMana = 100;
    private const float _manaPerSecond = 0.5f;

    public GameObject[] _iconObjects = new GameObject[6];

    #endregion

    #region Monobehaviour Function
    private void Awake()
    {
        if (null == Manager.Get<GameManager>().GetPlayerUnits()) { return; }

        _player1Base.GetComponent<SpawnManager>()._teamUnits 
            = Manager.Get<GameManager>().GetPlayerUnits();

        _timerUITime = int.Parse(_timerText.text);

        for (int i = 0; i < 6;++i)
        {
            int headNum = _player1Base.GetComponent<SpawnManager>()._teamUnits.GetUnit(i)._equipedItems[0];
            UnitIconManager.Update(_iconObjects[i], headNum);
        }
    }

    private void OnEnable()
    {
        _victoryObject.SetActive(false);
        _defeatObject.SetActive(false);
    }

    private void Update()
    {
        UpdateTimer();
        UpdateBase();
        UpdateGoldMana();
    }
    #endregion

    #region Private Function

    public GameObject _victoryObject;
    public GameObject _defeatObject;

    void Victory()
    {
        _victoryObject.SetActive(true);
    }

    void Defeat()
    {
        _defeatObject.SetActive(false);
    }

    private void UpdateTimer()
    {
        _timerUITime -= Time.deltaTime;

        _timerText.text = $"{(int)_timerUITime}";
    }

    private void UpdateBase()
    {
        _player1Bar.fillAmount = _player1Base.GetCurHealth() / _player1Base.GetMaxHealth();
        if(_player1Base.GetCurHealth() <= 0)
            Victory();

        _player2Bar.fillAmount = _player2Base.GetCurHealth() / _player2Base.GetMaxHealth();
        if (_player2Base.GetCurHealth() <= 0)
            Defeat();
    }

    private void UpdateGoldMana()
    {
        _curGold = Mathf.Clamp(_curGold + Time.deltaTime * _goldPerSecond, 0, _maxGold);
        _curMana = Mathf.Clamp(_curMana + Time.deltaTime * _manaPerSecond, 0, _maxMana);

        _goldImage.fillAmount = _curGold / _maxGold;
        _manaImage.fillAmount = _curMana / _maxMana;
    } 
    #endregion
}
