using UnityEngine;
using GameplayIngredients;
using TMPro;
using GameItem;

public class ItemSlot : MonoBehaviour
{
    private int _itemNum = -1;
    public int ItemNumber
    {
        get => _itemNum;
        set
        {
            _itemNum = value;
            UpdateText();
        }
    }

    [SerializeField]
    ItemSlotDefault _defaultObject;
    [SerializeField]
    ItemSlotInfo _infoObject;

    bool _isSelect = false;

    public void OffSelect()
    {
        _isSelect = false;

        //if (!_isSelect)
        { // Default
            _infoObject.gameObject.SetActive(false);
            _defaultObject.gameObject.SetActive(true);
        }

        UpdateText();
    }

    public void OnSelect()
    {
        _isSelect = true;

        //if (_isSelect)
        { // Info
            _infoObject.gameObject.SetActive(true);
            _defaultObject.gameObject.SetActive(false);
        }

        UpdateText();
    }

    private void Awake()
    {
        _defaultObject._slot = this;
        _infoObject._slot = this;
    }

    private void OnEnable()
    {
        OffSelect();
    }

    private void Start()
    {
        if (_isSelect)
        { // Info
            _infoObject.gameObject.SetActive(true);
            _defaultObject.gameObject.SetActive(false);
        }
        else
        { // Default
            _infoObject.gameObject.SetActive(false);
            _defaultObject.gameObject.SetActive(true);
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (!_isSelect)
        {
            _defaultObject.UpdateText();
        }
        else
        {
            _infoObject.UpdateText();

        }
    }

    void UpdateIcon()
    {
    }
}
