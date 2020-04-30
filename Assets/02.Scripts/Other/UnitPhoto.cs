using System.Collections;
using UnityEngine;

public class UnitPhoto : MonoBehaviour
{
    [System.Serializable]
    public class PartNameToObj
    {
        public string HeadName,BodyName,LeftWeaponName,RightWeaponName;
    }

    public PartNameToObj partNameToObj;
    
    [Space,SerializeField]
    private RenderTexture renderTexture;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //테스트용 유니티 폴더 경로
            StartCoroutine(Iphoto($"{Application.dataPath}/04.Image/Photo"));
            
            //실제 릴리즈 폴더 경로
            //StartCoroutine(Iphoto($"{Application.persistentDataPath}"));
        }
    }

    private readonly WaitForSeconds SideTime = new WaitForSeconds(0.1f);

    private IEnumerator Iphoto(string path)
    {
        Util.SaveRenderTextuerToPng(
            $"{path}/{partNameToObj.HeadName}-head,{partNameToObj.BodyName}-body,{partNameToObj.LeftWeaponName}-leftWeapon,{partNameToObj.RightWeaponName}-rightWeapon.png", renderTexture);

        //중간 텀
        yield return SideTime;
    }
}
