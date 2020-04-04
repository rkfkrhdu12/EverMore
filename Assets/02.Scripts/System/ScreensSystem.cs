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

    private void Start() => //초기에 로비 책갈피가 선택되있도록 함.
        UpdateButton(_startAccentBtn);

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

    //버튼 UI 이벤트로 할당 함.
    public void UpdateButton(Image image)
    {
        if (image == null)
        {
            Debug.LogError("UpdateButton(), The image was not assign.");
            return;
        }

        UpdateSprite(image);
    }

    private void UpdateSprite(Image ui)
    {
        #region 이전 UI에 대한 처리

        //이전 UI가 null이 아니라면, 이전 UI에 이미지가 선택되기 전 UI로 되돌려준다.
        if (_prevUI != null)
            _prevUI.sprite = _prevSprite;

        #endregion

        #region 현재 UI에 대한 처리

        //이전 UI에 현재 선택된 UI를 넣어준다.
        _prevUI = ui;

        //강조 이미지로 변경하기 전의 이미지를 할당해준다.
        _prevSprite = _prevUI.sprite;

        var btn = ui.GetComponent<Button>();

        //현재 선택된 UI에 참조된, 강조 이미지를 가져와 준다.
        Sprite changeSprite = btn.spriteState.selectedSprite;

        //현재 선택된 UI를 강조 이미지로 변경한다. 
        ui.sprite = changeSprite;

        //선택 상태로 변경
        btn.Select();

        #endregion
    }

    public void NextGoto(string scene) =>
        Manager.Get<SceneManagerPro>().LoadScene(scene);
}
