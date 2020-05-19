using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using GameplayIngredients;

using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    // public void SetAddTeamName(string teamName) { _addTeamName = teamName; }

    public void SetSelectUnitEquipedItems(int[] equipedItems)
    {
        //_teams[_curSelectTeamName].GetUnit(_curSelectUnitNum)._equipedItems = equipedItems;

        _teams[_curSelectTeamName].SetEquipedItems(_curSelectUnitNum,equipedItems);
        _teams[_curSelectTeamName].GetUnit(_curSelectUnitNum).UpdateItems();
    } 

    public UnitPhoto GetUnitPhoto() { return _unitPhoto; }

    #region Private Variable

    [SerializeField]
    private UnitPhoto _unitPhoto = null;

    //[SerializeField]
    //private MainSceneUI.MainSceneManager _sceneMgr = null;

    //[SerializeField]
    //private TeamSelectionSystem _teamSelectSystem = null;

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();


    [Header((MainSceneUI.UIDataKey.ChoiceUnitTeam) + " UI")]

    #region ChoiceUnitTeam
    [SerializeField, Tooltip("현재 선택된 팀 이름")]
    private string _curSelectTeamName;

    [SerializeField, Tooltip("유닛슬롯")]
    private RawImage[] _slotImage;

    [SerializeField]
    private Sprite _UnitAddImage;

    [SerializeField, Tooltip("팀이름 UI")]
    private TMP_Text _choiceUnitTeamNameUI = null;

    //[SerializeField]
    //private TeamSelectionSystem _teamSelectionSystem;

    #endregion

    //[Header((MainSceneUI.UIDataKey.AddTeam) + " UI")]
    
    //#region AddTeam
    //[SerializeField]
    //private TMP_InputField _teamAddInputField;

    //[SerializeField, Tooltip("추가될 팀 이름")]
    //private string _addTeamName; 
    //#endregion

    [Header(MainSceneUI.UIDataKey.SetUnit + " UI")]
    
    #region SetUnit
    private int _curSelectUnitNum = 0;

    //[SerializeField]
    //private GameObject _SetUnitObject = null;

    [SerializeField]
    private ItemInventorySystem _itemInventory = null; 
    #endregion

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
        // _addTeamName = "예비1팀";

        // AddTeam();

        string[] teamNames = new string[3];
        teamNames[0] = "예비1팀";
        teamNames[1] = "예비2팀";
        teamNames[2] = "예비3팀";

        for (int i = 0; i < 3; ++i)
        {
            Team t = new Team();
            t.Name = teamNames[i];
            t.Init();

            _teams.Add(teamNames[i], t);
            _teamNameList.Add(teamNames[i]);
        }

        //Manager.Get<GameManager>().SetPlayerUnits(_teams[_teamNameList[0]]);

        //for (int i = 0; i < _slotImage.Length; ++i)
        //{
        //    Image im = _slotImage[i].transform.parent.GetComponent<Image>();

        //    im.alphaHitTestMinimumThreshold = 0.5f;
        //}
    }

    private void OnEnable()
    {
        _curSelectTeamName = _teamNameList[0];
    }

    #endregion

    #region OnClickEvent Function

    public void OnSelectTeam(int teamIndex)
    {
        if(teamIndex >= _teamNameList.Count) { return; }

        _curSelectTeamName = _teamNameList[teamIndex];

        _choiceUnitTeamNameUI.text = _teams[_curSelectTeamName].Name;

        UpdateChoiceUnitsUI();
    }

    // SetUnit
    public void OnUpdateChoiceUnitUI()
    {
        UpdateChoiceUnitUI();
    }

    public void OnSelectUnit(int index)
    {
        if (index >= _teams[_curSelectTeamName].Length || index < 0) { return; }

        _curSelectUnitNum = index;

        UnitStatus curSelectUnit = _teams[_curSelectTeamName].GetUnit(_curSelectUnitNum);

        _itemInventory.SetEquipedItems(curSelectUnit._equipedItems);
    }

    public Team GetSelectTeam()
    {
        if (string.IsNullOrWhiteSpace(_curSelectTeamName))  { return null; }
        if (_teams[_curSelectTeamName].Length == 0)         { return null; }


        return _teams[_curSelectTeamName];
    }
    #endregion

    #region Private Function

    // ChoiceUnit
    private void UpdateChoiceUnitUI(int index = -1)
    {
        int selectUnitNum = index == -1 ? _curSelectUnitNum : index;

        UnitStatus unit = _teams[_curSelectTeamName].GetUnit(selectUnitNum) ;
        RawImage rawImage = _slotImage[selectUnitNum];

        if ((unit._equipedItems[0] == 0 && unit._equipedItems[1] == 0) || (unit._equipedItems[0] == 1 && unit._equipedItems[1] == 2))
        { // 알몸 상태 혹은 초기화가 안된상태 = _UnitAddImage.texture
            rawImage.texture = _UnitAddImage.texture;
            rawImage.SetNativeSize();
        }
        else
        {
            _unitPhoto.UpdateTexture(ref rawImage, unit._equipedItems);

            StartCoroutine(UnitTextureWaiting(rawImage));

            //if (rawImage.texture == null)
            //{
            //    rawImage.texture = _UnitAddImage.texture;
            //    rawImage.SetNativeSize();
            //}
            //else
            //{
            //    rawImage.SetNativeSize();
            //    rawImage.rectTransform.sizeDelta *= .625f;
            //}
        }
    }

    private readonly WaitForSeconds SideTime = new WaitForSeconds(0.075f);
    private IEnumerator UnitTextureWaiting(RawImage rawImage)
    {
        //중간 텀
        yield return SideTime;

        if (rawImage.texture == null)
        {
            rawImage.texture = _UnitAddImage.texture;
            rawImage.SetNativeSize();
        }
        else
        {
            rawImage.SetNativeSize();
            rawImage.rectTransform.sizeDelta *= .6f;
        }
    }

    private void UpdateChoiceUnitsUI()
    {
        int unitCount = _teams[_curSelectTeamName].Length;

        if (unitCount > _slotImage.Length) { Debug.Log("TeamManager : OnUpdateUI UnitCount Error"); return; }

        for (int i = 0; i < _slotImage.Length; ++i)
        {
            UpdateChoiceUnitUI(i);

            //UnitStatus unit = _teams[_curSelectTeamName].GetUnit(i);

            //if ((unit._equipedItems[0] == 0 && unit._equipedItems[1] == 0) || (unit._equipedItems[0] == 1 && unit._equipedItems[1] == 2))
            //{ // 알몸 상태 혹은 초기화가 안된상태 = _UnitAddImage.texture
            //    _slotImage[i].texture = _UnitAddImage.texture;
            //    _slotImage[i].SetNativeSize();
            //}
            //else
            //{
            //    _unitPhoto.UpdateTexture(ref _slotImage[i], unit._equipedItems);
            //    // _slotImage[i].rectTransform.sizeDelta = new Vector2(320.0f, 320.0f);

            //    if(_slotImage[i].texture == null)
            //    {
            //        _slotImage[i].texture = _UnitAddImage.texture;
            //        _slotImage[i].SetNativeSize();
            //    }
            //}
        }
    }

    #endregion
}