using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public void SetSelectUnitEquipedItems(int[] equipedItems)
    {
        _teams[_curSelectTeamName].SetEquipedItems(_curSelectUnitNum,equipedItems);
        _teams[_curSelectTeamName].GetUnit(_curSelectUnitNum).UpdateItems();
    } 

    public UnitPhoto GetUnitPhoto() { return _unitPhoto; }

    #region OnClickEvent Function

    public void OnSelectTeam(int teamIndex)
    {
        if (teamIndex >= _teamNameList.Count) { return; }

        _curSelectTeamName = _teamNameList[teamIndex];

        _choiceUnitTeamNameUI.text = _teams[_curSelectTeamName].Name;

        UpdateChoiceUnitsUI();
    }

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
        if (string.IsNullOrWhiteSpace(_curSelectTeamName)) { return null; }
        if (_teams[_curSelectTeamName].Length == 0) { return null; }


        return _teams[_curSelectTeamName];
    }
    #endregion

    #region Private Variable

    [SerializeField]
    private UnitPhoto _unitPhoto = null;

    // 팀 이름으로 팀을 찾는다
    private Dictionary<string, Team> _teams = new Dictionary<string, Team>();
    private List<string> _teamNameList = new List<string>();

    #region ChoiceUnitTeam
    [Header((MainSceneUI.UIDataKey.ChoiceUnitTeam) + " UI")]

    [SerializeField, Tooltip("현재 선택된 팀 이름")]
    private string _curSelectTeamName;

    [SerializeField]
    private GameObject[] _unitSlots;

    [SerializeField]
    private Sprite _UnitAddImage;

    [SerializeField, Tooltip("팀이름 UI")]
    private TMP_Text _choiceUnitTeamNameUI = null;

    #endregion

    #region SetUnit
    [Header(MainSceneUI.UIDataKey.SetUnit + " UI")]

    private int _curSelectUnitNum = 0;
    [SerializeField]
    private ItemInventorySystem _itemInventory = null; 
    #endregion

    #endregion

    #region Monobehaviour Function

    private void Awake()
    {
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
    }

    private void OnEnable()
    {
        _curSelectTeamName = _teamNameList[0];
    }

    #endregion

    #region Private Function

    // ChoiceUnit
    private void UpdateChoiceUnitUI(int index = -1)
    {
        int selectUnitNum = index == -1 ? _curSelectUnitNum : index;

        UnitStatus unit = _teams[_curSelectTeamName].GetUnit(selectUnitNum) ;
        RawImage rawImage = null;

        if ((rawImage = _unitSlots[selectUnitNum].transform.GetChild(0).GetChild(0).GetComponent<RawImage>())) { }
        else { Debug.Log("UpdateChoiceUnitUI : Unit RawImage Load Error"); return; } 
        
        if ((unit._equipedItems[0] == 0 && unit._equipedItems[1] == 0) || (unit._equipedItems[0] == 1 && unit._equipedItems[1] == 2))
        { // 알몸 상태 혹은 초기화가 안된상태 = _UnitAddImage.texture
            rawImage.texture = _UnitAddImage.texture;
            rawImage.SetNativeSize();
        }
        else
        {
            _unitPhoto.UpdateTexture(ref rawImage, unit._equipedItems);

            UnitIconManager.Update(_unitSlots[selectUnitNum].transform.GetChild(1).gameObject, unit._equipedItems[0]);

            StartCoroutine(UnitTextureWaiting(rawImage));
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

        if (unitCount > _unitSlots.Length) { Debug.Log("TeamManager : OnUpdateUI UnitCount Error"); return; }

        for (int i = 0; i < _unitSlots.Length; ++i)
        {
            UpdateChoiceUnitUI(i);
        }
    }

    #endregion
}