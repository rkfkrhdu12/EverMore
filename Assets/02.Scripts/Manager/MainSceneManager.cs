using System.Collections.Generic;
using GameplayIngredients;
using UnityEngine;

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

    public class UIScreen
    {
        public GameObject myUI;

        //해당 챕터가 꺼져있는지 켜져있는지 체크합니다.
        public bool activeSelf;

        /// <summary>
        /// 해당 UI챕터를 켜거나 끕니다.
        /// </summary>
        /// <param name="active"></param>
        public void setActive(bool active)
        {
            activeSelf = active;

            myUI?.SetActive(active);
        }
    }

    public class MainSceneManager : MonoBehaviour
    {
        #region Show Inspector

        [Header("팀을 고를 UI 오브젝트들")]
        [SerializeField, Tooltip("팀을 고를 UI 오브젝트")]
        private GameObject _choiceTeamUI;

        [SerializeField, Tooltip("팀을 추가하는 UI 오브젝트")]
        private GameObject _addTeamUI;

        [Header("유닛을 고를 UI 오브젝트들")]
        [SerializeField, Tooltip("유닛을 고를 UI 오브젝트")]
        private GameObject _choiceUnitUI;

        [Header("유닛을 세팅할 UI 오브젝트들")]
        [SerializeField, Tooltip("유닛을 세팅할 UI 오브젝트")]
        private GameObject _setUnitUI;

        #endregion

        // UIScreenObject 의 이름, List에서의 번호
        private Dictionary<string, UIScreen> nameToSceenUI = new Dictionary<string, UIScreen>();

        private string _curKey = string.Empty;

        private void Awake()
        {
            //UI키 : UI 오브젝트 형태로 링크해줍니다.
            InitUI(UIDataKey.Lobby);
            InitUI(UIDataKey.ChoiceTeam, _choiceTeamUI);
            InitUI(UIDataKey.AddTeam, _addTeamUI);
            InitUI(UIDataKey.ChoiceUnit, _choiceUnitUI);
            InitUI(UIDataKey.SetUnit, _setUnitUI);
        }

        private void InitUI(string key, GameObject ui = null)
        {
            //객체 생성
            var uiScreen = new UIScreen();

            //인자로 넘어온 UI가 null이 아닐 경우, UI스크린 리스트에 수록
            if (ui != null) uiScreen.myUI = ui;

            //딕셔너리에 해당 키와 UI스크린의 리스트를 생성하고, UI 스크린 객체를 수록한다.
            nameToSceenUI[key] = uiScreen;

            //방금 Add한 오브젝트를 비활성화 시킴
            nameToSceenUI[key].setActive(false);
        }

        public void UpdateScreen(string key)
        {
            //인자로 넘어온 키 값이 카운트에 없을 경우 : return
            if (!nameToSceenUI.ContainsKey(key))
                return;

            //현재 활성화된 UI는 비활성화 한다.
            if (!_curKey.Equals(string.Empty))
                nameToSceenUI[_curKey].setActive(false);

            // //새로 들어온 Key의 UI를 활성화 한다.
            nameToSceenUI[key].setActive(true);
            _curKey = key;
        }

        public void UpdateCheckScreen(string key)
        {
            //인자로 넘어온 키 값이 카운트에 없을 경우 : return
            if (!nameToSceenUI.ContainsKey(key))
                return;

            //해당 UI스크린을 가져온다.
            UIScreen ui = nameToSceenUI[key];

            //활성화 <-> 비활성화
            ui.setActive(!ui.activeSelf);
        }

        public void NextScreen()
        {
            switch (_curKey)
            {
                case UIDataKey.ChoiceTeam:
                    UpdateScreen(UIDataKey.ChoiceUnit);
                    break;
            }
        }

        public void PrevScreen()
        {
            switch (_curKey)
            {
                case UIDataKey.ChoiceTeam:
                    UpdateScreen(UIDataKey.Lobby);
                    break;
                case UIDataKey.ChoiceUnit:
                    UpdateScreen(UIDataKey.ChoiceTeam);
                    break;
            }
        }

        public void NextGoto(string scene) =>
            Manager.Get<SceneManagerPro>().LoadScene(scene);
    }
}
