using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eItemInvenType
{
    HELMET,
    BODYARMOUR,
    WEAPON,
}

public class ItemInventoryManager : MonoBehaviour
{
    private List<int>[] _inventory = new List<int>[3];

    eItemInvenType _curType = eItemInvenType.HELMET;
    public eItemInvenType Type { get { return _curType; }
        set
        {
            if (value != _curType)
            {
                _curType = value;
                UpdateInventory();
            }
        }
    }
    public void TypeHelmet()        { Type = eItemInvenType.HELMET; }
    public void TypeBodyArmour()    { Type = eItemInvenType.BODYARMOUR; }
    public void TypeWeapon()        { Type = eItemInvenType.WEAPON; }

    // Set Inventory    
    //public void Set

    // null 은 inspector를 통해 저장
    public GameObject _inventoryObject = null;

    public GameObject _itemPrefab = null;

    private void Awake()
    {
        _inventory[(int)eItemInvenType.HELMET]      = new List<int>();

        _inventory[(int)eItemInvenType.BODYARMOUR]  = new List<int>();

        _inventory[(int)eItemInvenType.WEAPON]      = new List<int>();
    }

    private void Start()
    {
        // Test
        {
            TypeItemAllSet(eCodeType.HELMET, eItemInvenType.HELMET);
            TypeItemAllSet(eCodeType.BODYARMOUR, eItemInvenType.BODYARMOUR);
            TypeItemAllSet(eCodeType.WEAPON, eItemInvenType.WEAPON);
        }


        Type = eItemInvenType.HELMET;
        UpdateInventory();
    }

    // Test
    void TypeItemAllSet(eCodeType codeType,eItemInvenType itemType)
    {
        ItemList itemList = GameSystem.Instance.itemList;

        int count = itemList.ItemCount(codeType);

        for (int i = 0; i < count; ++i) 
        {
            int num = itemList.CodeSearch(codeType, i);

            _inventory[(int)itemType].Add(num);
        }
    }

    private void OnEnable()
        // Start
    {
        Debug.Log("OnEnable");

        Type = eItemInvenType.HELMET;
        UpdateInventory();
    }

    void UpdateInventory()
    {
        int curType = (int)_curType;

        int curUseItemSlotCount = _inventory[curType].Count;

        if (0 >= curUseItemSlotCount) { return; }
        if (null == _itemPrefab || null == _inventoryObject) { return; }

        int prevUseItemSlotCount = _inventoryObject.transform.childCount;

        int i = 0;

        int emptySlotCount = prevUseItemSlotCount - curUseItemSlotCount;
        if (0 < emptySlotCount)
        {
            for (int j = 0; j < emptySlotCount; ++j)
            {
                DeleteObjectManager.AddDeleteObject(_inventoryObject.transform.GetChild(0).gameObject);
            }
        }
        else if (0 > emptySlotCount)
        {
            for (int j = 0; j > emptySlotCount; --j) 
            {
                GameObject clone = Instantiate(_itemPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), _inventoryObject.transform);
            }
        }

        for (; i < curUseItemSlotCount; ++i)
        {
            _inventoryObject.transform.GetChild(i).GetComponent<ItemSlot>().ItemNumber
                = _inventory[curType][i];
        }
    }
}
