using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameplayIngredients;

namespace Stage
{
    public struct SpawnUnit
    {
        public int SpawnSec;
        public int Index;
        public bool IsDown;
    }
    public class StageData
    {
        public Team Team;

        public List<SpawnUnit> _spawnUnitData;

        public void Init()
        {
            Team = new Team();
            Team.Init(eTeam.ENEMY);

            _spawnUnitData = new List<SpawnUnit>();
        }
    }

    [Serializable]
    public class StageUIData
    {
        public RawImage[] _enemyImages;

        public GameObject[] _headIconObjects;
    }

    public class StageManager : MonoBehaviour
    { // 현재 적 레벨 관리와 정보를 인게임에 전달

        public void OnStageEasy()
        {
            Manager.Get<GameManager>().SetEnemyUnitData(_stageDataList[0]);
        }

        public void OnStageNormal()
        {
            Manager.Get<GameManager>().SetEnemyUnitData(_stageDataList[2]);
        }

        public void OnStageHard()
        {
            Manager.Get<GameManager>().SetEnemyUnitData(_stageDataList[1]);
        }

        public void StageClear()
        {
            _curStage = Mathf.Min(_stageDataList.Count, _curStage + 1);
            _isStageUpdate = false;

            Manager.Get<GameManager>().SetEnemyUnitData(_stageDataList[_curStage]);
        }
        
        // Test
        public Team GetTeam(int index) { return _stageDataList[index].Team; }

        public StageData GetStageData() { return _curStage < _stageDataList.Count ? _stageDataList[_curStage] : null; }

        public void OnUpdateUI()
        {
            if (_stageDataList.Count == 0)
                Init();

            StageData curData = _stageDataList[_curStage];
        }

        #region Variable
        public int _curStage = 0;

        List<StageData> _stageDataList = new List<StageData>();

        [SerializeField]
        private StageUIData _stageUIs = new StageUIData();

        [SerializeField]
        private UnitPhoto _unitPhoto = null;

        [SerializeField]
        private GameObject _modelObject = null;

        bool _isStageUpdate = false;
        bool _isInit = false;
        #endregion

        #region Monobehaviour Function

        private void Start()
        {
            Init();

            Manager.Get<GameManager>().SetEnemyUnitData(_stageDataList[_curStage]);
        }

        #endregion

        #region Private Function

        void Init()
        {
            if(_isInit) { return; }
            _isInit = true;

            InitStage("Stage01");
            InitStage("Stage02");
            InitStage("Stage03");
        }

        private void InitStage(string fileName)
        {
            List<string> stageDatas = CSVParser.Read(fileName);
            if (stageDatas == null) { return; }

            int lineCount = 0;
            string[] splitDatas;

            StageData newData = new StageData();
            newData.Init();

            // 유닛 갯수는 최대 6개
            for (; lineCount < 6; ++lineCount)
            {
                splitDatas = stageDatas[lineCount].Split(',');

                for (int j = 0; j < 4; ++j)
                {
                    int.TryParse(splitDatas[j], out newData.Team.GetUnit(lineCount)._equipedItems[j]);
                }
            }
            ++lineCount;

            splitDatas = stageDatas[lineCount++].Split(',');

            while (!string.IsNullOrWhiteSpace(splitDatas[0]) && stageDatas.Count > lineCount)
            {
                SpawnUnit newUnit = new SpawnUnit();
                int.TryParse(splitDatas[0], out newUnit.SpawnSec);
                int.TryParse(splitDatas[1], out newUnit.Index);
                int updownNum; int.TryParse(splitDatas[2], out updownNum);
                newUnit.IsDown = updownNum == 0 ? false : true;

                newData._spawnUnitData.Add(newUnit);

                splitDatas = stageDatas[lineCount++].Split(',');
            }

            _stageDataList.Add(newData);
        }
        #endregion
    }
}
