using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;

public enum eItemInvenType
{
    HELMET,
    BODYARMOUR,
    WEAPON,
}

public class ItemInventorySystem : MonoBehaviour
{
    public GameObject _contentObject;

    public GameObject _itemPrefab;

    public eItemInvenType Type
    {
        get => _curType;
        set
        {
            if (_curType == value) return;

            _curType = value;
            UpdateInventory();
        }
    }

    #region Hide Inspector

    private eItemInvenType _curType = eItemInvenType.HELMET;
    private readonly List<int>[] _inventory = new List<int>[3];

    #endregion

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

    //매번 UI가 활성화 될 때, 호출됩니다.
    private void OnEnable()
    {
        Type = eItemInvenType.HELMET;
        UpdateInventory();
    }

    // Test
    private void TypeItemAllSet(eCodeType codeType, eItemInvenType itemType)
    {
        //아이템 리스를 가져온다.
        ItemList itemList = Manager.Get<GameManager>().itemList;

        //아이템 개수 반환.
        int count = itemList.ItemCount(codeType);

        //아이템 개수만큼 반복,
        for (int i = 0; i < count; ++i)
        {
            int num = itemList.CodeSearch(codeType, i);

            _inventory[(int) itemType].Add(num);
        }
    }

    /// <summary>
    /// 타입을 헤멧을 만듭니다.
    /// </summary>
    public void TypeHelmet() => Type = eItemInvenType.HELMET;

    /// <summary>
    /// 타입을 아머로 만듭니다.
    /// </summary>
    public void TypeBodyArmour() => Type = eItemInvenType.BODYARMOUR;

    /// <summary>
    /// 타입을 무기로 만듭니다.
    /// </summary>
    public void TypeWeapon() => Type = eItemInvenType.WEAPON;

    private void UpdateInventory()
    {
        //현재 타입이 무엇인지 가져옵니다.
        int curType = (int) _curType;

        //현재 사용중인 아이템 슬롯의 개수를 가져옵니다.
        int curUseItemSlotCount = _inventory[curType].Count;

        //현재 아이템 슬롯 개수가 0이하라면 : return
        if (curUseItemSlotCount <= 0)
            return;

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
                Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity, _contentObject.transform);

        //현재 슬롯 개수만큼 반복하여, 아이템 넘버를 변경한다.
        for (int i = 0; i < curUseItemSlotCount; ++i)
            _contentObject.transform.GetChild(i).GetComponent<ItemSlot>().ItemNumber = _inventory[curType][i];
    }
}
