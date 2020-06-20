using UnityEngine;
using GameplayIngredients;
using TMPro;

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

    public TMP_Text _text;

    int prevItemIconObject = -1;

    private void Awake() => UpdateText();

    private void UpdateText()
    {
        GameItem.Item item = Manager.Get<GameManager>().itemList.ItemSearch(_itemNum);
        
        if (item == null)
            return;

        if (_text == null)
            _text = transform.GetChild(transform.childCount - 1).GetComponent<TMP_Text>();

        int childNum = -1;
        switch (item.AniType)
        {
            case GameItem.eItemType.Helmet:     childNum = 0; break;
            case GameItem.eItemType.BodyArmour: childNum = 1; break;
        }

        if (prevItemIconObject != -1)
            UnitIconManager.Reset(transform.GetChild(prevItemIconObject).gameObject);

        if (childNum != -1)
        {
            transform.GetChild(childNum).gameObject.SetActive(true);

            prevItemIconObject = childNum;
            IconUpdate(childNum, item.Name);
        }

        _text.text = item.Name;
    }

    void IconUpdate(int iconObjChildCount, string ItemName)
    {
        UnitIconManager.Reset(transform.GetChild(iconObjChildCount).gameObject);

        UnitIconManager.Update(transform.GetChild(iconObjChildCount).gameObject, ItemName);
    }
}
