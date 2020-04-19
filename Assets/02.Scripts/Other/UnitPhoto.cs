using System.Collections;
using UnityEngine;

public class UnitPhoto : MonoBehaviour
{
    [System.Serializable]
    private class PartNameToObj
    {
        public string name;

        public GameObject head;

        public GameObject body;
    }

    [SerializeField]
    private RenderTexture renderTexture;

    [SerializeField]
    private PartNameToObj[] partNameToObj;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
            StartCoroutine(Iphoto($"{Application.dataPath}/04.Image/Photo"));
    }

    private readonly WaitForSeconds SideTime = new WaitForSeconds(0.1f);
    
    private IEnumerator Iphoto(string path)
    {
        //전체 유닛의 사진을 찍어야하기 때문에 유닛 개수만큼 반복합니다.
        for (int i = 0; i < partNameToObj.Length; i++)
        {
            //풀샷을 찍습니다.
            partNameToObj[i].head.SetActive(true);
            partNameToObj[i].body.SetActive(true);

            //중간 텀
            yield return SideTime;
            
            //김치
            Util.SaveRenderTextuerToPng(
                $"{path}/{partNameToObj[i].name}-head,{partNameToObj[i].name}-body.png", renderTexture);
            
            //해당 유닛의 바디를 가립니다.
            partNameToObj[i].body.SetActive(false);

            //남은 파츠를 찍습니다.
            for (int j = 0; j < partNameToObj.Length; j++)
            {
                if (j == i) continue;
                
                //다른 파츠 바디를 보이게함.
                partNameToObj[j].body.SetActive(true);
                
                //중간 텀
                yield return SideTime;
                
                //치즈~
                Util.SaveRenderTextuerToPng(
                    $"{path}/{partNameToObj[i].name}-head,{partNameToObj[j].name}-body.png", renderTexture);
                
                //중간 텀
                yield return SideTime;
                
                //다른 바디가 보일 수 있도록 해당 바디를 가립니다.
                partNameToObj[j].body.SetActive(false);
            }

            //해당 유닛 파츠 머리르 가립니다.
            partNameToObj[i].head.SetActive(false);
        }

        //전체를 다시 비활성화 합니다.
        foreach (var t in partNameToObj)
        {
            t.head.SetActive(false);
            t.body.SetActive(false);
        }
    }
}
