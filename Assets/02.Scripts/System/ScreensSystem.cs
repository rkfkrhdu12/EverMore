using System;
using System.Collections;
using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreensSystem : MonoBehaviour
{
    #region Enum

    private enum eScreenType
    {
        LOBBY,
        TEAM,
        UNIT,
    }

    #endregion

    #region Show Inspector

    [Header("초기에 강조할 버튼 UI")]
    [SerializeField, Tooltip("초기에 강조할 버튼")]
    private Image _startAccentBtn;

    [Header("유닛을 세팅할 UI 오브젝트")]
    [SerializeField, Tooltip("유닛을 세팅할 UI 오브젝트")]
    private GameObject _unitSetting;

    [Header("유닛 들")]
    [SerializeField, Tooltip("유닛들")]
    private GameObject _units;

    #endregion

    #region Hide Inspector

    //이전에 선택된 UI
    private Image _prevUI;

    //이전에 선택된 UI가 선택 이미지로 변경되기 전 이미지
    private Sprite _prevSprite;

    private eScreenType _curType = eScreenType.LOBBY;
    private const string _teamScreen = "Team";

    #endregion
    
    //버튼 UI 이벤트로 할당 함.
    public void UpdateScreen(string type)
    {
        switch (type)
        {
            case _teamScreen:

                //유닛 세팅 창을 오픈합니다.
                _unitSetting.SetActive(true);
                _units.SetActive(true);
                
                //
                // //유닛 개수만큼 반복,
                // for (int i = 0; i < Manager.Get<GameManager>().getPlayerUnitCount(); ++i)
                // {
                //     // 장비는 4개이므로 4번 반복,
                //     for (int j = 0; j < 4; ++j)
                //     {
                //         //비교할 아이템 번호를 가져온다.
                //         int equipNum = Manager.Get<GameManager>().getPlayerUnit(i)._itemsNum[j];
                //
                //         //아이템 번호가 0이라면 : continue
                //         if (equipNum == 0)
                //             continue;
                //
                //         //해당 아이템 번호로 아이템을 검색한다.
                //         var item = Manager.Get<GameManager>().itemList.ItemSearch(equipNum);
                //
                //         //아이템이 가져와지지 않았다면 : continue
                //         if (item == null)
                //             continue;
                //
                //         //아이템 오브젝트 : 생성
                //         Instantiate(item.Object, _units.transform.GetChild(i));
                //     }
                // }
                //
              

                break;
        }
    }
    
    public void NextGoto(string scene) =>
        Manager.Get<SceneManagerPro>().LoadScene(scene);
}
