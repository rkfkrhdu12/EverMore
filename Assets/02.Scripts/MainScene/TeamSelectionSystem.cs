using System;
using TMPro;
using UnityEngine;

public class TeamSelectionSystem : MonoBehaviour
{
    #region Show Inspector

    [Header("생성될 버튼의 오브젝트")]
    [SerializeField]
    private GameObject _buttonPrefab;

    [SerializeField, Tooltip("버튼 그룹처리")]
    private ButtonGroup buttonGroup;

    #endregion

    public void AddButton(TMP_InputField tmp)
    {
        //버튼을 추가하고, 해당 버튼의 엘리멘트를 가져옵니다.
        var btn = Instantiate(_buttonPrefab, transform).GetComponent<TeamElemnt>();
        
        //버튼의 글자를 변경합니다.
        btn.btnText.text = tmp.text;
        
        //버튼 그룹에 해당 버튼을 추가합니다.
        buttonGroup.AddButton(btn.buttonPro);
        
        //빈칸으로 초기화
        tmp.text = string.Empty;
    }
}
