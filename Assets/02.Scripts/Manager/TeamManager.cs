using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using GameplayIngredients;

using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public void SetAddTeamName(string teamName) { _addTeamName = teamName; }

    public void SetSelectUnitEquipedItems(int[] equipedItems)
    {
        _teams[_curSelectTeamName].GetUnit(_curSelectUnitNum)._equipedItems = equipedItems;
        _teams[_curSelectTeamName].GetUnit(_curSelectUnitNum).UpdateItems();
    } 

    public UnitPhoto GetUnitPhoto() { return _unitPhoto; }

    #region Private Variable

    [SerializeField]
    private UnitPhoto _unitPhoto = null;

    [SerializeField]
    private MainSceneUI.MainSceneManager _sceneMgr = null;

    [SerializeField]
    private TeamSelectionSystem _teamSelectSystem = null;

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();


    [Header((MainSceneUI.UIDataKey.ChoiceUnit) + " UI")] 
 
    #region ChoiceUnit
    [SerializeField, Tooltip("현재 선택된 팀 이름")]
    private string _curSelectTeamName;

    [SerializeField, Tooltip("유닛슬롯")]
    private RawImage[] _slotImage;

    [SerializeField]
    private Sprite _UnitAddImage;

    [SerializeField, Tooltip("팀이름 UI")]
    private TMP_Text _choiceUnitTeamNameUI = null;

    [SerializeField]
    private TeamSelectionSystem _teamSelectionSystem;

    #endregion

    [Header((MainSceneUI.UIDataKey.AddTeam) + " UI")]
    
    #region AddTeam
    [SerializeField]
    private TMP_InputField _teamAddInputField;

    [SerializeField, Tooltip("추가될 팀 이름")]
    private string _addTeamName; 
    #endregion

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
        _addTeamName = "예비1팀";

        AddTeam();

        Manager.Get<GameManager>().SetPlayerUnits(_teams[_teamNameList[0]]);
    }

    #endregion

    #region OnClickEvent Function

    // Choice Team
    public void OnSelectTeam(TMP_Text text)         
    {
        SelectTeamName(text);
        UpdateChoiceUnitUI();
        SetTeamNameUI();
    }
    
    // Add Team
    public void OnAddTeam()                         
    {
        if(_teams.ContainsKey(_addTeamName)) { return; }

        AddTeam();

        _teamSelectionSystem.AddButton(_teamAddInputField);
    }

    // Choice Unit
    public void OnDeleteTeam()                      
    {
        // Team 데이터는 가비지 컬렉터가 지워줄꺼라 믿숩니다 ............
        _teams.Remove(_curSelectTeamName);
    }

    // SetUnit
    public void OnUpdateChoiceUnitUI()              
    { UpdateChoiceUnitUI(); }
    public void OnSelectUnit(int index)
    {
        if (index >= _teams[_curSelectTeamName].Length || index < 0) { return; }

        _curSelectUnitNum = index;

        UnitStatus curSelectUnit = _teams[_curSelectTeamName].GetUnit(_curSelectUnitNum);

        //UnitModelManager UMMgr = new UnitModelManager();
        //UMMgr.UpdateModel(_SetUnitObject, curSelectUnit._equiedItems);

        _itemInventory.SetEquipedItems(curSelectUnit._equipedItems);
    }

    public Team GetSelectTeam()
    {
        if(string.IsNullOrWhiteSpace(_curSelectTeamName)) { return null; }

        return _teams[_curSelectTeamName];
    }
    #endregion

    #region Private Function

    // ChoiceTeam
    private void SelectTeamName(TMP_Text text)  
    {
        string teamName = text.text;

        if (string.IsNullOrEmpty(teamName)) { return; }
        if (!_teams.ContainsKey(teamName)) { return; }

        _curSelectTeamName = teamName;
    }
    private void SetTeamNameUI()                
    {
        _choiceUnitTeamNameUI.text = _teams[_curSelectTeamName].Name;
    }

    // AddTeam
    private void AddTeam()                      
    {
        if (_teams.ContainsKey(_addTeamName)) { return; }

        string teamName = _addTeamName;

        Team t = new Team();
        t.Name = teamName;
        t.Init();

        _teams.Add(teamName, t);
        _teamNameList.Add(teamName);
    }

    // ChoiceUnit
    private void UpdateChoiceUnitUI()
    {
        int unitCount = _teams[_curSelectTeamName].Length;

        if (unitCount > _slotImage.Length) { Debug.Log("TeamManager : OnUpdateUI UnitCount Error"); return; }

        for (int i = 0; i < _slotImage.Length; ++i)
        {
            UnitStatus unit = _teams[_curSelectTeamName].GetUnit(i);

            if ((unit._equipedItems[0] == 0 && unit._equipedItems[1] == 0) || (unit._equipedItems[0] == 1 && unit._equipedItems[1] == 2))
            { // 알몸 상태 혹은 초기화가 안된상태 = _UnitAddImage.texture
                _slotImage[i].texture = _UnitAddImage.texture;
                _slotImage[i].SetNativeSize();
            }
            else
            {
                Debug.Log("UpdateTexture : " + unit._equipedItems[0] + " " + unit._equipedItems[1]);
                _unitPhoto.UpdateTexture(ref _slotImage[i], unit._equipedItems);
                _slotImage[i].rectTransform.sizeDelta = new Vector2(256.0f, 256.0f);

                if(_slotImage[i].texture == null)
                {
                    _slotImage[i].texture = _UnitAddImage.texture;
                    _slotImage[i].SetNativeSize();
                }
            }
        }
    }

    #endregion
}