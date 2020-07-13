using UnityEngine;
using GameplayIngredients;
using TMPro;
using GameItem;
using UnityEngine.UI;

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
        if(!_isSelect) { return; }
        _isSelect = false;

        _infoSlot.gameObject.SetActive(false);
        _defaultSlot.gameObject.SetActive(true);

        _defaultSlot.UpdateText();
    }

    public void OnSelect()
    {
        if(_isSelect) { return; }
        _isSelect = true;

        _infoSlot.gameObject.SetActive(true);
        _defaultSlot.gameObject.SetActive(false);

        _infoSlot.UpdateText();
    }

    private void Awake()
    {
        _defaultSlot._slot = this;
        _infoSlot._slot = this;

        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _isSelect = !GetComponent<ButtonPro>().isSelected;

        if (!_isSelect)
            OnSelect();
        else
            OffSelect();
    }

    private void Start()
    {
        _defaultSlot._itemList = _invenSystem._itemList;
        _infoSlot._itemList = _invenSystem._itemList;
    }

    [SerializeField] Sprite[] _spriteState;
    Image _image;

    private void FixedUpdate()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(_spriteState.Length < 2) { return; }

        if (_image.sprite == _spriteState[0])
        {
            OffSelect();
        }
        else if(_image.sprite == _spriteState[1])
        {
            OnSelect();
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
