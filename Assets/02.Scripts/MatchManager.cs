using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public struct SpawnUnit
    {
        public int SpawnSec;
        public int Index;
        public bool IsDown;
    }

    class StageData
    {
        public Team Team;

        public List<SpawnUnit> _spawnSecList;

        public void Init()
        {
            Team = new Team();
            Team.Init(eTeam.ENEMY);

            _spawnSecList = new List<SpawnUnit>();
        }
    }

    public class MatchManager : MonoBehaviour
    { // 현재 적 레벨 관리와 정보를 인게임에 전달

        List<StageData> _stageDataList = new List<StageData>();

        private void Awake()
        {
            InitStage("Stage01");

        }


        void InitStage(string fileName)
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

            splitDatas = stageDatas[lineCount++].Split(',');

            while (!string.IsNullOrWhiteSpace(splitDatas[0]))
            {
                splitDatas = stageDatas[lineCount++].Split(',');

                SpawnUnit newUnit = new SpawnUnit();
                int.TryParse(splitDatas[0], out newUnit.SpawnSec);
                int.TryParse(splitDatas[1], out newUnit.Index);
                int updownNum; int.TryParse(splitDatas[2], out updownNum);
                newUnit.IsDown = updownNum == 0 ? false : true;

                newData._spawnSecList.Add(newUnit);
            }
        }
    }
}
