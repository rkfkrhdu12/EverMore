using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manager
using GameplayIngredients;

namespace UIManager
{
    public struct UIDataKey
    {
        public const string ChoiceTeam = "ChoiceTeam";
        public const string AddButton = "AddButton";
        public const string ChoiceUnit = "ChoiceUnit";
        public const string SetUnit = "SetUnit";
    }

    public class ScreenManager : MonoBehaviour
    {
        #region Show Inspector

        [Header("팀을 고를 UI 오브젝트들")]
        [SerializeField, Tooltip("팀을 고를 UI 오브젝트")] 
        private GameObject _choiceTeamUI = null;
        [SerializeField, Tooltip("팀을 추가하는 UI 오브젝트")]
        private GameObject _addTeamUI = null;


        [Header("유닛을 고를 UI 오브젝트들")]
        [SerializeField, Tooltip("유닛을 고를 UI 오브젝트")]
        private GameObject _choiceUnitUI = null;

        [Header("유닛을 세팅할 UI 오브젝트들")]
        [SerializeField, Tooltip("유닛을 세팅할 UI 오브젝트")]
        private GameObject _setUnitUI = null;
        [SerializeField, Tooltip("3D 유닛 UI 오브젝트")]
        private GameObject _unitObjectUI = null;

        #endregion

        // UIScreenObject 의 이름, List에서의 번호
        private Dictionary<string, int> _count = new Dictionary<string, int>();
        private int _prevKey = 0;

        // UIScreen
        private List<UIScreen> _list = new List<UIScreen>();

        private void Awake()
        {
            InitUI(UIDataKey.ChoiceTeam, _choiceTeamUI, _addTeamUI);
            InitUI(UIDataKey.ChoiceUnit, _choiceUnitUI);
            InitUI(UIDataKey.SetUnit   , _setUnitUI, _unitObjectUI);
        }

        void InitUI(string key,GameObject ui1,GameObject ui2 = null)
        {
            UIScreen ui = new UIScreen();
            ui._UIList.Add(ui1);
            if (null != ui2)
                ui._UIList.Add(ui2);

            _count.Add(key, _list.Count);
            _list.Add(ui);
        }

        public void UpdateScreen(string key)
        {
            if(!_count.ContainsKey(key)) { return; }

            _list[_prevKey].Disable();

            _prevKey = _count[key];
            _list[_prevKey].Enable();

        }


        //#region Hide Inspector

        //private static eScreenType _curType = eScreenType.Lobby;
        //private const string _teamScreenName = "Team";

        //#endregion

        ////버튼 UI 이벤트로 할당 함.



        //private void Start()
        //{
        //    DisableChoiceTeamScreen();
        //    DisableAddTeamScreen();
        //    DisableChoiceUnitScreen();
        //    DisableSetUnitScreen();
        //}

        //public void NextScreen()
        //{
        //    switch (_curType)
        //    {
        //        case eScreenType.ChoiceTeam: ChangeScreen(eScreenType.ChoiceUnit); break;
        //        case eScreenType.AddTeam: ChangeScreen(eScreenType.ChoiceTeam); break;
        //        case eScreenType.ChoiceUnit: ChangeScreen(eScreenType.SetUnit); break;
        //    }
        //}

        //private void ChangeScreen(eScreenType type)
        //{
        //    switch (_curType)
        //    {
        //        case eScreenType.Lobby: break;
        //        case eScreenType.ChoiceTeam: DisableChoiceTeamScreen(); break;
        //        case eScreenType.AddTeam: DisableAddTeamScreen(); break;
        //        case eScreenType.ChoiceUnit: DisableChoiceUnitScreen(); break;
        //        case eScreenType.SetUnit: DisableSetUnitScreen(); break;
        //    }

        //    _curType = type;

        //    switch (_curType)
        //    {
        //        case eScreenType.Lobby: break;
        //        case eScreenType.ChoiceTeam: EnableChoiceTeamScreen(); break;
        //        case eScreenType.ChoiceUnit: EnableChoiceUnitScreen(); break;
        //        case eScreenType.SetUnit: EnableSetUnitScreen(); break;
        //    }
        //}

        //#region Team & Unit Setting

        //void EnableChoiceTeamScreen() { _choiceTeamUI.SetActive(true); }
        //void DisableChoiceTeamScreen() { _choiceTeamUI.SetActive(false); }

        //void EnableAddTeamScreen() { _addTeamUI.SetActive(true); }
        //void DisableAddTeamScreen() { _addTeamUI.SetActive(false); }

        //void EnableChoiceUnitScreen() { _choiceUnitUI.SetActive(true); }
        //void DisableChoiceUnitScreen() { _choiceUnitUI.SetActive(false); }

        //void EnableSetUnitScreen() { _setUnitUI.SetActive(true); _unitObjectUI.SetActive(true); }
        //void DisableSetUnitScreen() { _setUnitUI.SetActive(false); _unitObjectUI.SetActive(false); }

        //#endregion

        public void NextGoto(string scene) =>
            Manager.Get<SceneManagerPro>().LoadScene(scene);
    }

    class UIScreen
    {
        public List<GameObject> _UIList = new List<GameObject>(); 

        public void Enable() { foreach (GameObject i in _UIList) { i.SetActive(true); } }
        public void Disable() { foreach (GameObject i in _UIList) { i.SetActive(false); } }
    }

}