using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manager
using GameplayIngredients;

namespace MainScene
{
    public struct UIDataKey
    {
        public const string Lobby = "Lobby";
        public const string ChoiceTeam = "ChoiceTeam";
        public const string AddTeam = "AddTeam";
        public const string ChoiceUnit = "ChoiceUnit";
        public const string SetUnit = "SetUnit";
    }

    public class MainSceneManager : MonoBehaviour
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
        // private Stack<string> _curKeyStack = new Stack<string>();
        private string _curKey = ""; // { get { return _curKeyStack.Peek(); } }

        // UIScreen
        private List<UIScreen> _list = new List<UIScreen>();

        private void Awake()
        {
            InitUI(UIDataKey.Lobby);
            InitUI(UIDataKey.ChoiceTeam, _choiceTeamUI);
            InitUI(UIDataKey.AddTeam, _addTeamUI);
            InitUI(UIDataKey.ChoiceUnit, _choiceUnitUI);
            InitUI(UIDataKey.SetUnit   , _setUnitUI, _unitObjectUI);
        }

        void InitUI(string key,GameObject ui1 = null,GameObject ui2 = null)
        {
            UIScreen ui = new UIScreen();
            if (null != ui1)
                ui._UIList.Add(ui1);
            if (null != ui2)
                ui._UIList.Add(ui2);

            _count.Add(key, _list.Count);
            _list.Add(ui);
            _list[_count[key]].Disable();
        }

        public void UpdateScreen(string key)
        {
            if(!_count.ContainsKey(key)) { return; }

            if (_count.ContainsKey(_curKey))
                _list[_count[_curKey]].Disable();

            _curKey = key;
            _list[_count[_curKey]].Enable();
        }

        public void UpdateCheckScreen(string key)
        {
            if (!_count.ContainsKey(key)) { return; }

            UIScreen ui = _list[_count[key]];

            if (ui._isOn) ui.Disable();
            else          ui.Enable();
        }

        public void NextScreen()
        {
            switch(_curKey)
            {
                case UIDataKey.Lobby:                                           break;
                case UIDataKey.ChoiceTeam: UpdateScreen(UIDataKey.ChoiceUnit);  break;
                case UIDataKey.ChoiceUnit:                                      break;
            }
        }

        public void PrevScreen()
        {
            switch (_curKey)
            {
                case UIDataKey.Lobby:                                           break;
                case UIDataKey.ChoiceTeam:  UpdateScreen(UIDataKey.Lobby);      break;
                case UIDataKey.ChoiceUnit:  UpdateScreen(UIDataKey.ChoiceTeam); break;
            }
        }

        public void NextGoto(string scene) =>
            Manager.Get<SceneManagerPro>().LoadScene(scene);
    }

    class UIScreen
    {
        public List<GameObject> _UIList = new List<GameObject>();

        public bool _isOn = false;

        public void Enable()
        {
            _isOn = true;
            foreach (GameObject i in _UIList)
            {
                i.SetActive(_isOn);
            }
        }

        public void Disable()
        {
            _isOn = false;
            foreach (GameObject i in _UIList)
            {
                i.SetActive(_isOn);
            }
        }
    }

}