using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    Sprite _prevSprite  = null;
    Image _prevUI       = null;

    public Image _lobbyButtonImage = null;

    public GameObject[] _unitSettingScreen;

    GameManager _gSystem;

    eScreenType _curType = eScreenType.LOBBY;
    private enum eScreenType
    {
        LOBBY,
        TEAM,
        UNIT,
    }

    const string _teamScreen = "Team";

    private void Start()
    {
        _gSystem = GameManager.Instance;

    }

    private void OnEnable()
    {
        ResetSprite();

        UpdateButton(_lobbyButtonImage);
    }

    public void UpdateButton(Image image)
    {
        if(null == image) { Debug.LogError("UpdateButton() Error"); return; }

        UpdateSprite(image);
    }

    public void ResetSprite()
    {
        if (null == _prevUI) { return; }

        _prevUI.sprite = _prevSprite;

        _prevUI = null;
    }

    private void UpdateSprite(Image ui)
    {
        if (null == ui) { Debug.LogError("UpdateSprite() Error"); return; }

        if (null != _prevUI)
        {
            _prevUI.sprite = _prevSprite;
        }
        _prevUI = ui;

        Sprite changeSprite = ui.GetComponent<Button>().spriteState.selectedSprite;

        _prevSprite = ui.sprite;
        ui.sprite = changeSprite;
    }

    public void UpdateScreen(string type)
    {
        switch(type)
        {
            case _teamScreen:
            _unitSettingScreen[0].SetActive(true);

            for (int i = 0; i < _gSystem.GetPlayerUnitCount(); ++i) 
            {
                // 장비는 4개 
                for (int j = 0; j < 4; ++j)
                {
                    int equipNum = _gSystem.GetPlayerUnit(i)._itemsNum[j];

                    if(0 == equipNum) { continue; }

                    Item item = _gSystem.itemList.ItemSearch(equipNum);

                    // 후에 알몸이나 빈것을 만들도록 수정
                    if (null == item) { continue; }

                    Instantiate(item.Object, _unitSettingScreen[1].transform.GetChild(i));
                }

                //
            }

            _unitSettingScreen[1].SetActive(true);

            break;
        }
    }
}
