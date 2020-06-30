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
    ItemSlotDefault _defaultSlot;
    [SerializeField]
    ItemSlotInfo _infoSlot;

    [SerializeField]
    ItemInventorySystem _invenSystem;

    bool _isSelect = false;

    public void OffSelect()
    {
        _isSelect = false;

        //if (!_isSelect)
        { // Default
            _infoSlot.gameObject.SetActive(false);
            _defaultSlot.gameObject.SetActive(true);
        }

        UpdateText();
    }

    public void OnSelect()
    {
        _isSelect = true;

        //if (_isSelect)
        { // Info
            _infoSlot.gameObject.SetActive(true);
            _defaultSlot.gameObject.SetActive(false);
        }

        UpdateText();
    }

    private void Awake()
    {
        _defaultSlot._slot = this;
        _infoSlot._slot = this;
    }

    private void OnEnable()
    {
        OffSelect();
    }

    private void Start()
    {
        _defaultSlot._itemList = _invenSystem._itemList;
        _infoSlot._itemList = _invenSystem._itemList;
    }

    private void UpdateText()
    {
        if (!_isSelect)
        {
            _defaultSlot.UpdateText();
        }
        else
        {
            _infoSlot.UpdateText();

        }
    }

    void UpdateIcon()
    {
    }
}
