using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using GameplayIngredients;
using GameItem;

public class ItemSlotDefault : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    ItemList _itemList;

    [HideInInspector]
    public ItemSlot _slot;

    int _prevIconObjectIndex = -1;

    public void UpdateText()
    {
        if (_itemList == null) { _itemList = Manager.Get<GameManager>().itemList; }

        Item item = _itemList.ItemSearch(_slot.ItemNumber);

        if (item == null) { return; }
        if (_text == null) { _text = transform.GetChild(transform.childCount - 1).GetComponent<TMP_Text>(); }

        UpdateIcon(item);

        _text.text = item.Name;
    }

    void UpdateIcon(Item item)
    {
        if (_prevIconObjectIndex != -1)
        { UnitIconManager.Reset(transform.GetChild(_prevIconObjectIndex).gameObject); }

        int iconObjChildCount;
        switch (item.AniType)
        {
            case eItemType.Helmet: iconObjChildCount = 0; break;
            case eItemType.BodyArmour: iconObjChildCount = 1; break;
            default: iconObjChildCount = 2; break;
        }

        GameObject curIconObject = transform.GetChild(iconObjChildCount).gameObject;

        if (!curIconObject.activeSelf)
        { curIconObject.SetActive(true); }

        UnitIconManager.Reset(curIconObject);
        UnitIconManager.Update(curIconObject, item.Name);

        _prevIconObjectIndex = iconObjChildCount;
    }
}
