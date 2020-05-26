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

    [SerializeField]
    private TeamManager _teamManager = null;

    [SerializeField]
    private GameObject _unitModelUI = null;

    [SerializeField]
    private ButtonGroup _itemsButtonGroup;

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

    private eCodeType _curType = eCodeType.HELMET;
    private readonly List<int>[] _inventory = new List<int>[3];

    private ItemList _itemList;

    private int[] _equipedItems = new int[4];

    #endregion

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        _equipedItems[0] = 1;
        _equipedItems[1] = 2;
        _equipedItems[2] = 0;
        _equipedItems[3] = 0;

        _unitPhoto = _teamManager.GetUnitPhoto();
        if (null == _unitPhoto) { Debug.LogError("ItemInventorySystem : _unitPhoto is null"); }

        _itemList = Manager.Get<GameManager>().itemList;

        _inventory[(int)eCodeType.HELMET] = new List<int>();

        _inventory[(int)eCodeType.BODYARMOUR] = new List<int>();

        _inventory[(int)eCodeType.WEAPON] = new List<int>();
    }

    private void Start()
    {
        #region Test

        List<string> armourList = new List<string>();

        GameItem.eCodeType helmet = GameItem.eCodeType.HELMET;
        GameItem.eCodeType armour = GameItem.eCodeType.BODYARMOUR;

        for (int i = 0; i < _itemList.GetCodeItemCount(helmet); ++i)
        {
            int code = -1;
            if ((code = _itemList.CodeSearch(helmet, i)) != -1)
                armourList.Add(_itemList.ItemSearch(code).Name);

            if ((code = _itemList.CodeSearch(armour, i)) != -1)
                armourList.Add(_itemList.ItemSearch(code).Name);
        }

        eCodeType[] codes = new eCodeType[2];
        codes[0] = eCodeType.HELMET;
        codes[1] = eCodeType.BODYARMOUR;
        int num;
        for (int i = 0; i < armourList.Count; ++i)
        {
            num = _itemList.CodeSearch(codes[i % 2], armourList[i]);

            _inventory[(int)codes[i % 2]].Add(num);
        }

        List<string> weaponList = new List<string>();

        GameItem.eCodeType weapon = GameItem.eCodeType.WEAPON;
        for (int i = 0; i < _itemList.GetCodeItemCount(weapon); ++i)
        {
            int code = _itemList.CodeSearch(weapon, i);

            weaponList.Add(_itemList.ItemSearch(code).Name);
        }

        weaponList.Remove("빛의 포크");

        for (int i = 0; i < weaponList.Count; ++i) 
        {
            num = _itemList.CodeSearch(eCodeType.WEAPON, weaponList[i]);

            _inventory[(int)eCodeType.WEAPON].Add(num);
        }

        #endregion

        Type = eCodeType.HELMET;

        UpdateInventory();
    }

    //매번 UI가 활성화 될 때, 호출됩니다.
    private void OnEnable()
    {
        Type = eCodeType.HELMET;

        UpdateInventory();
    } 
    #endregion

    public void TypeHelmet()        => Type = eCodeType.HELMET;
    public void TypeBodyArmour()    => Type = eCodeType.BODYARMOUR;
    public void TypeRightWeapon()   { Type = eCodeType.WEAPON; _isLeftWeapon = false; }
    public void TypeLeftWeapon()    { Type = eCodeType.WEAPON; _isLeftWeapon = true; }

    public void SetEquipedItems(int[] items)
    {
        UnitModelManager.Reset(_unitModelUI, _equipedItems);

        // 유닛 아이템들이 유닛1 수정완료(5,6)   유닛2 수정완료(5,6)

        _equipedItems = new int[4];
        for (int i = 0; i < 4; ++i)
            _equipedItems[i] = items[i];

        UpdateInventory();
    }

    public void OnSave()
    {
        _unitPhoto.SaveTexture(_equipedItems);
        _teamManager.SetSelectUnitEquipedItems(_equipedItems);
    }

    public void OnEquiped(ItemSlot curSlot)
    {
        int itemCode = curSlot.ItemNumber;

        Item i = _itemList.ItemSearch(itemCode);
        if (i == null) { Debug.Log("ItemInventorySystem : UpdateEquipedItem i is Error"); return; }

        // 모델링 업데이트
        {
            int partsNum = 0;
            switch (i.Type)
            {
                case eItemType.NONE: Debug.Log("ItemInventorySystem : UpdateEquipedItem i.Type is Error"); return;
                case eItemType.HELMET: partsNum = 0; break;
                case eItemType.BODYARMOUR: partsNum = 1; break;
                default: partsNum = (_isLeftWeapon ? 2 : 3); break;
            }

            if (partsNum >= _equipedItems.Length) { Debug.Log("ItemInventorySystem : UpdateEquipedItem partsNum is Error"); return; }

            int prevItem = _equipedItems[partsNum];
            _equipedItems[partsNum] = itemCode;

            UpdateModel(prevItem);
        }

        {
            if (_curType != eCodeType.WEAPON) return;

            UnitAnimationManager.Update(_equipedItems[2], _equipedItems[3], _unitModelUI.GetComponent<Animator>());
        }
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

                curSlot.GetComponent<ItemSlot>().ItemNumber = _inventory[curType][i];
                _itemsButtonGroup.AddButton(curSlot.GetComponent<ButtonPro>());

                if (_inventory[curType][i] == _equipedItems[curType])
                { // 창작한 아이템코드가 이번 아이템코드
                    _itemsButtonGroup.SelectButton(curSlot.GetComponent<ButtonPro>());
                }
                
            }

            // _itemsButtonGroup.SelectedNumber = -1;
        }

        UpdateModel();

        UnitAnimationManager.Update(_equipedItems[2], _equipedItems[3], _unitModelUI.GetComponent<Animator>());
    }

    void UpdateModel(in int prevItem = 0)
    {
        if (_equipedItems == null && _unitModelUI == null) { return; }
        if (_equipedItems.Length == 0) { return; }

        UnitModelManager.Update(_unitModelUI, _equipedItems, prevItem);
    }

    #endregion
}
