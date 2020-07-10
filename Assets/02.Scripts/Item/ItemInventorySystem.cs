using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;

using GameItem;

public class ItemInventorySystem : MonoBehaviour
{
    #region Variable
    public GameObject _contentObject;

    public GameObject _itemPrefab = null;

    private UnitPhoto _unitPhoto;
    public Animator _saveAnimator;

    ItemSlot _prevSlot;

    [SerializeField]
    private TeamManager _teamManager = null;

    [SerializeField]
    private GameObject _unitModelObject = null;

    [SerializeField]
    private ButtonGroup _itemsButtonGroup = null;

    [SerializeField]
    private GameObject _unitDetailUI = null;

    public eCodeType Type   
    {
        get => _curType;
        set
        {
            if (_curType == value) return;

            _curType = value;
            UpdateInventory();
        }
    }

    /// <summary>
    /// 왼쪽은 true[3] 오른쪽은 false[4]
    /// </summary>
    private bool _isLeftWeapon = true;

    #region Hide Inspector

    private eCodeType _curType = eCodeType.Weapon;
    private readonly List<int>[] _inventory = new List<int>[4];

    public ItemList _itemList;

    private int[] _equipedItems = new int[4];

    #endregion

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        _unitPhoto = _teamManager.GetUnitPhoto();

        if (null == _unitPhoto) { Debug.LogError("ItemInventorySystem : _unitPhoto is null"); }

        _itemList = Manager.Get<GameManager>().itemList;

        _inventory[(int)eCodeType.Helmet]       = new List<int>();

        _inventory[(int)eCodeType.Bodyarmour]   = new List<int>();

        _inventory[(int)eCodeType.RightWeapon]  = new List<int>();

        _inventory[(int)eCodeType.LeftWeapon] = new List<int>();

        _equipedItems[0] = _itemList.CodeSearch(eCodeType.Helmet, 0);
        _equipedItems[1] = _itemList.CodeSearch(eCodeType.Bodyarmour, 0);
        _equipedItems[2] = 0;
        _equipedItems[3] = 0;

        _unitDetailUI.SetActive(false);
    }

    bool isTesting = false;
    private void Start()
    {
        if (isTesting) { return; }

        #region Test
        List<string> armourList = new List<string>();

        eCodeType helmet = eCodeType.Helmet;
        eCodeType armour = eCodeType.Bodyarmour;

        for (int i = 0; i < _itemList.GetCodeItemCount(helmet); ++i)
        {
            int code = -1;
            if ((code = _itemList.CodeSearch(helmet, i)) != -1)
                armourList.Add(_itemList.ItemSearch(code).Name);

            if ((code = _itemList.CodeSearch(armour, i)) != -1)
                armourList.Add(_itemList.ItemSearch(code).Name);
        }

        eCodeType[] codes = new eCodeType[2];
        codes[0] = eCodeType.Helmet;
        codes[1] = eCodeType.Bodyarmour;
        int num;
        for (int i = 0; i < armourList.Count; ++i)
        {
            num = _itemList.CodeSearch(codes[i % 2], armourList[i]);

            _inventory[(int)codes[i % 2]].Add(num);
        }

        List<string> weaponList = new List<string>();

        eCodeType weapon = eCodeType.Weapon;
        for (int i = 0; i < _itemList.GetCodeItemCount(weapon); ++i)
        {
            int code = _itemList.CodeSearch(weapon, i);

            weaponList.Add(_itemList.ItemSearch(code).Name);
        }

        for (int i = 0; i < weaponList.Count; ++i)
        {
            num = _itemList.CodeSearch(eCodeType.Weapon, weaponList[i]);

            _inventory[(int)eCodeType.LeftWeapon].Add(num);
            _inventory[(int)eCodeType.RightWeapon].Add(num);
        }

        isTesting = true;
        #endregion
    }

    //매번 UI가 활성화 될 때, 호출됩니다.
    private void OnEnable()
    {
        Type = eCodeType.Helmet;

        UnitModelManager.Reset(_unitModelObject);
        UnitModelManager.Update(_unitModelObject, _equipedItems);

        Start();
    }
    #endregion

    public void TypeHelmet()        { Type = eCodeType.Helmet;     }
    public void TypeBodyArmour()    { Type = eCodeType.Bodyarmour; }
    public void TypeRightWeapon()   { Type = eCodeType.LeftWeapon; _isLeftWeapon = false; }
    public void TypeLeftWeapon()    { Type = eCodeType.RightWeapon; _isLeftWeapon = true; }

    public void SetEquipedItems(int[] items)
    {
        UnitModelManager.Reset(_unitModelObject, _equipedItems);

        // 유닛 아이템들이 유닛1 수정완료(5,6)   유닛2 수정완료(5,6)

        _equipedItems = new int[4];
        for (int i = 0; i < 4; ++i)
            _equipedItems[i] = items[i];

        UpdateInventory();

        UpdateModel();

        UnitAnimationManager.Update(_equipedItems[2], _equipedItems[3], _unitModelObject.GetComponent<Animator>());
    }

    public void OnSave()
    {
        _unitPhoto.SaveTexture(_equipedItems);
        _teamManager.SetSelectUnitEquipedItems(_equipedItems);

        _saveAnimator.SetTrigger("OnAnimation");
    }

    public void OnEquiped(ItemSlot curSlot)
    {
        if (_prevSlot != null)
        {
            _prevSlot.OffSelect();
        }

        curSlot.OnSelect();

        int itemCode = curSlot.ItemNumber;

        Item i = _itemList.ItemSearch(itemCode);
        if (i == null) { LogMessage.Log("ItemInventorySystem : UpdateEquipedItem i is Error"); return; }

        // 모델링 업데이트

        UpdateModel(i.AniType, itemCode);

        if ((_curType == eCodeType.LeftWeapon || _curType == eCodeType.RightWeapon))
        { UnitAnimationManager.Update(_equipedItems[2], _equipedItems[3], _unitModelObject.GetComponent<Animator>()); }

        _prevSlot = curSlot;
    }

    public void OnDetailUIEnable()
    {
        

        _unitDetailUI.SetActive(true);
    }

    public void OnDetailUIDisable()
    {


        _unitDetailUI.SetActive(false);
    }

    #region Private Function

    private void UpdateInventory()
    {
        int curType = (int)_curType;
        // Update Slot
        {
            for (int i = 0; i < _inventory[curType].Count; ++i)
                if (_itemList.ItemSearch(_inventory[curType][i]) == null) _inventory[curType].Remove(_inventory[curType][i--]);

            //현재 사용중인 아이템 슬롯의 개수를 가져옵니다.
            int curUseItemSlotCount = _inventory[curType].Count;

            //아이템이 없거나, 오브젝트 컨텐츠가 없을시 : return
            if (_itemPrefab == null || _contentObject == null)
                return;

            //이전 아이템 슬롯의 개수를 가져옵니다.
            int prevUseItemSlotCount = _contentObject.transform.childCount;

            //비어있는 슬롯 개수 = 이전 개수 - 현재 개수
            int emptySlotCount = prevUseItemSlotCount - curUseItemSlotCount;

            //비어있는 슬롯 개수가 있다면, 비어있는 슬롯 개수만큼 제거
            if (emptySlotCount > 0)
                for (int j = 0; j < emptySlotCount; ++j)
                    DeleteObjectSystem.AddDeleteObject(_contentObject.transform.GetChild(0).gameObject);

            //반대로 슬롯 개수가 0 미만이라면, 아이템 프리팹을 추가합니다.
            else if (emptySlotCount < 0)
                for (int j = 0; j > emptySlotCount; --j)
                    Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _contentObject.transform).SetActive(true);

            int selectedSlot = _itemsButtonGroup.GetSelectNumber();
            if (selectedSlot != -1)
                _itemsButtonGroup.buttonPros[selectedSlot].isSelected = false;

            _itemsButtonGroup.buttonPros = new List<ButtonPro>();

            //현재 슬롯 개수만큼 반복하여, 아이템 넘버를 변경한다.
            for (int i = 0; i < curUseItemSlotCount; ++i)
            {
                GameObject curSlot = _contentObject.transform.GetChild(i).gameObject;
                curSlot.SetActive(false);

                ItemSlot slot = curSlot.GetComponent<ItemSlot>();
                slot.ItemNumber = _inventory[curType][i];
                _itemsButtonGroup.AddButton(curSlot.GetComponent<ButtonPro>());

                curSlot.SetActive(true);

                if (_inventory[curType][i] == _equipedItems[curType])
                { 
                    _itemsButtonGroup.SelectButton(curSlot.GetComponent<ButtonPro>());

                    slot.OnSelect();

                    _prevSlot = slot;
                }
            }
        }
    }

    private void UpdateModel()
    {
        if (_equipedItems == null && _unitModelObject == null) { return; }
        if (_equipedItems.Length == 0) { return; }
        
        UnitModelManager.Update(_unitModelObject, _equipedItems);
    }

    private void UpdateModel(eItemType itemType, int itemCode)
    {
        if (_equipedItems == null && _unitModelObject == null) { return; }
        if (_equipedItems.Length == 0) { return; }

        int partsNum;
        switch (itemType)
        {
            case eItemType.None: return;
            case eItemType.Helmet: partsNum = 0; break;
            case eItemType.BodyArmour: partsNum = 1; break;
            default: partsNum = (_isLeftWeapon ? 2 : 3); break;
        }

        if (partsNum >= _equipedItems.Length) { return; }

        int prevItem = _equipedItems[partsNum];

        if (_equipedItems[partsNum] == itemCode)
        { // 이미 장착했음 > 장착해제
            int defaultItemCode = 0;
            switch (partsNum)
            {
                case 0: defaultItemCode = _itemList.CodeSearch(eCodeType.Helmet, 0); break;
                case 1: defaultItemCode = _itemList.CodeSearch(eCodeType.Bodyarmour, 0); break;
            }

            _equipedItems[partsNum] = defaultItemCode;
        }
        else
        { // 장착
            _equipedItems[partsNum] = itemCode;
        }
        
        UnitModelManager.Update(_unitModelObject, _equipedItems, prevItem);
    }

    #endregion
}
