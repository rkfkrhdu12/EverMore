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
        Item item = GameManager.Instance.itemList.ItemSearch(_itemNum);
        if(null == item) { return; }
        
        _text.text = item.Name;

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }
}
