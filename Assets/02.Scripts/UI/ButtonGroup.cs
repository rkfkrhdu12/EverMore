using System;
using UnityEditorInternal;
using UnityEngine;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField,Tooltip("그룹으로 지어줄 버튼들")]
    private ButtonPro[] buttonPros;

    [SerializeField, Tooltip("초기 선택될 버튼")]
    private int SelectedNumber = -1;

    private void Awake()
    {
        //버튼이 
        if(buttonPros.Length <1)
            return;

        foreach (var btnPro in buttonPros)
            btnPro.isSelected = false;

        if(SelectedNumber == -1)
            return;

        SelectedNumber = Mathf.Clamp(SelectedNumber, 0, buttonPros.Length);
        
        buttonPros[SelectedNumber].isSelected = true;
    }
}
