using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using GameplayIngredients;
using GameItem;

public class ItemSlotInfo : MonoBehaviour
{
    GameObject _prevSlot;

    private ItemList _itemList;

    [HideInInspector]
    public ItemSlot _slot;

    int _prevIconObjectIndex = -1;

    [SerializeField]
    private TMP_Text[] _statusTexts;
    enum eStatusState   
    {
        Health,
        Attack,
        Defence,
        CoolDown,
        Cost,
    }

    public void UpdateText()
    {
        if (_itemList == null) { _itemList = Manager.Get<GameManager>().itemList; }

        Item i = _itemList.ItemSearch(_slot.ItemNumber);

        if (i == null) { return; }

        _statusTexts[(int)eStatusState.Health].text     = i.Health.ToString();
        _statusTexts[(int)eStatusState.Attack].text     = i.MinDamage.ToString() + "~" + i.MaxDamage.ToString();
        _statusTexts[(int)eStatusState.Defence].text    = i.DefensePower.ToString();
        _statusTexts[(int)eStatusState.CoolDown].text   = i.CoolTime.ToString();
        _statusTexts[(int)eStatusState.Cost].text       = i.Cost.ToString();

        UpdateIcon(i);
    }

    void UpdateIcon(Item i)
    {
        if (_prevIconObjectIndex != -1) 
        { UnitIconManager.Reset(transform.GetChild(_prevIconObjectIndex).gameObject); }

        int iconObjChildCount;
        switch (i.AniType)
        {
            case eItemType.Helmet:      iconObjChildCount = 0; break;
            case eItemType.BodyArmour:  iconObjChildCount = 1; break;
            default:                    iconObjChildCount = 2; break;
        }

        GameObject curIconObject = transform.GetChild(iconObjChildCount).gameObject;

        if (!curIconObject.activeSelf)
            { curIconObject.SetActive(true); }

        UnitIconManager.Reset(curIconObject);
        UnitIconManager.Update(curIconObject, i.Name);

        _prevIconObjectIndex = iconObjChildCount;
    }
}
