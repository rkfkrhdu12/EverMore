using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDetailsUI : MonoBehaviour
{
    private int _itemNumber = -1;

    public int ItemNumber
    {
        get { return _itemNumber; }
        set { _itemNumber = value; UpdateText(); }
    }



    void UpdateText()
    {

    }
}
