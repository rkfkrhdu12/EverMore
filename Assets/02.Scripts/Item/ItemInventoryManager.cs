using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;

public enum eItemInvenType
{
    HELMET,
    BODYARMOUR,
    WEAPON,
}

public class ItemInventoryManager : MonoBehaviour
{
    private readonly List<int>[] _inventory = new List<int>[3];

    private eItemInvenType _curType = eItemInvenType.HELMET;

    public eItemInvenType Type
    {
        get => _curType;
        set
        {
            if (value == _curType) return;
            _curType = value;
            UpdateInventory();
        }
    }

    public void TypeHelmet()
    {
        Type = eItemInvenType.HELMET;
    }

    public void TypeBodyArmour()
    {
        Type = eItemInvenType.BODYARMOUR;
    }

    public void TypeWeapon()
    {
        Type = eItemInvenType.WEAPON;
    }

    // Set Inventory    
    //public void Set

    // null 은 inspector를 통해 저장
    public GameObject _contentObject ;

    public GameObject _itemPrefab ;

    private void Awake()
    {
        _inventory[(int) eItemInvenType.HELMET] = new List<int>();

        _inventory[(int) eItemInvenType.BODYARMOUR] = new List<int>();

        _inventory[(int) eItemInvenType.WEAPON] = new List<int>();
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
    void TypeItemAllSet(eCodeType codeType, eItemInvenType itemType)
    {
        ItemList itemList = Manager.Get<GameManager>().itemList;

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
        Type = eItemInvenType.HELMET;
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        int curType = (int) _curType;

        int curUseItemSlotCount = _inventory[curType].Count;

        if (0 >= curUseItemSlotCount)
        {
            return;
        }

        if (null == _itemPrefab || null == _contentObject)
            return;

        int prevUseItemSlotCount = _contentObject.transform.childCount;

        int i = 0;

        int emptySlotCount = prevUseItemSlotCount - curUseItemSlotCount;
        
        if (0 < emptySlotCount)
        {
            for (int j = 0; j < emptySlotCount; ++j)
                DeleteObjectSystem.AddDeleteObject(_contentObject.transform.GetChild(0).gameObject);
        }
        else if (0 > emptySlotCount)
        {
            for (int j = 0; j > emptySlotCount; --j)
            {
                var clone = Instantiate(_itemPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero),
                    _contentObject.transform);
            }
        }

        for (; i < curUseItemSlotCount; ++i)
        {
            _contentObject.transform.GetChild(i).GetComponent<ItemSlot>().ItemNumber
                = _inventory[curType][i];
        }
    }
}
