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

    private void Awake() => UpdateText();

    private void UpdateText()
    {
        Item item = Manager.Get<GameManager>().itemList.ItemSearch(_itemNum);
        
        if (item == null)
            return;

        _text.text = item.Name;

        // var localPosition = transform.localPosition;
        //
        // localPosition = new Vector3(localPosition.x, localPosition.y, 0);
        // transform.localPosition = localPosition;
    }
}
