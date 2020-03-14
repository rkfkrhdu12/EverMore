using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private int _itemNum = -1;
    public int ItemNumber { get { return _itemNum; } set { _itemNum = value; UpdateText(); } }

    public Text _text;

    private void Awake()
    {
        UpdateText();
    }

    void UpdateText() // 기획자님께서 잘 해주시겟죠 
    {
        Debug.Log("checking  " + _itemNum);
        Item item = GameSystem.Instance.itemList.ItemSearch(_itemNum);
        if(null == item) { return; }


        Debug.Log("pass  " + _itemNum);

        _text.text = item.Name;
    }
}
