using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperateButtonGroup : MonoBehaviour
{
    public ButtonPro GetTextMesh(int index)
    {
        if(_buttonList.Count <= index) { return null; }

        return _buttonList[index];
    }

    public void AddButton()
    {
        GameObject clone = Instantiate(_buttonPrefab, transform);

        UpdateButtonCount();
    }

    private void Awake()
    {
        UpdateButtonCount();
    }

    void UpdateButtonCount()
    {
        ButtonPro[] buttons = GetComponentsInChildren<ButtonPro>();

        _buttonCount = buttons.Length;

        _buttonList.Clear();

        foreach (ButtonPro i in buttons)
            _buttonList.Add(i);
    }

    #region Private Variable
    private List<ButtonPro> _buttonList = new List<ButtonPro>();

    [Header("현재 인식된 변수의 갯수")]
    [SerializeField]
    private int _buttonCount = 0;

    [Header("생성될 버튼의 오브젝트")]
    [SerializeField]
    private GameObject _buttonPrefab = null;
    #endregion
}
