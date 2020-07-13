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
        if (!_isSelect) { return; }

        _isSelect = false;

        _infoSlot.gameObject.SetActive(false);
        _defaultSlot.gameObject.SetActive(true);

        _defaultSlot.UpdateText();
    }

    public void OnSelect()
    {
        if (_isSelect) 

        _isSelect = true;

        _infoSlot.gameObject.SetActive(true);
        _defaultSlot.gameObject.SetActive(false);

        _infoSlot.UpdateText();
    }

    private void Awake()
    {
        _defaultSlot._slot = this;
        _infoSlot._slot = this;

        _buttonPro = GetComponent<ButtonPro>();
    }

    private void OnEnable()
    {
        _isSelect = true;

        OffSelect();
        _prevState = _buttonPro.isSelected;
    }

    private void Start()
    {
        _defaultSlot._itemList = _invenSystem._itemList;
        _infoSlot._itemList = _invenSystem._itemList;
    }

    ButtonPro _buttonPro;

    bool _prevState;

    private void FixedUpdate()
    {
        if(_buttonPro.isSelected != _prevState)
        {
            if(_buttonPro.isSelected)
            {
                OnSelect();
            }
            else
            {
                OffSelect();
            }
        }
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
    
}
