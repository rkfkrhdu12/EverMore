using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameplayIngredients;

public class ItemSlot : MonoBehaviour
{
    private int _itemNum = -1;
    public int ItemNumber { get { return _itemNum; } set { _itemNum = value; UpdateText(); } }

    public Text _text;

    private void Awake()
    {
        UpdateText();
    }

    void UpdateText() 
    {
        Item item = Manager.Get<GameManager>().itemList.ItemSearch(_itemNum);
        if (null == item) { return; }

        _text.text = item.Name;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
