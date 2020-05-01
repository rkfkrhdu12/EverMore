using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UnitPhoto : MonoBehaviour
{
    [System.Serializable]
    public class PartNameToObj
    {
        public string HeadName, BodyName, LeftWeaponName, RightWeaponName;
    }

    public PartNameToObj partNameToObj;

    [Space, SerializeField]
    private RenderTexture renderTexture;

    [SerializeField, Tooltip("이미지 가져오기 용도")]
    private RawImage rawimage;

    private void Update()
    {
        //저장하기
        if (Input.GetKeyDown(KeyCode.Space)) 
            StartCoroutine(Iphoto($"{Application.persistentDataPath}"));

        //불러오기
        if (Input.GetKeyDown(KeyCode.L)) 
            StartCoroutine(ILoadTexture());
        
        //텍스쳐가 저장된 폴더 경로
        if(Input.GetKeyDown(KeyCode.H))
            Debug.Log(Application.persistentDataPath);
    }

    

    private IEnumerator ILoadTexture()
    {

        using (var uwr = UnityWebRequestTexture.GetTexture($@"{Application.persistentDataPath}/{partNameToObj.HeadName}-head,{partNameToObj.BodyName}-body,{partNameToObj.LeftWeaponName}-leftWeapon,{partNameToObj.RightWeaponName}-rightWeapon.png"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
                Debug.Log(uwr.error);
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                rawimage.texture = texture;
            }
        }
    }

    private readonly WaitForSeconds SideTime = new WaitForSeconds(0.1f);

    private IEnumerator Iphoto(string path)
    {
        Util.SaveRenderTextuerToPng(
            $"{path}/{partNameToObj.HeadName}-head,{partNameToObj.BodyName}-body,{partNameToObj.LeftWeaponName}-leftWeapon,{partNameToObj.RightWeaponName}-rightWeapon.png",
            renderTexture);

        //중간 텀
        yield return SideTime;

        Debug.Log("찍힘");
    }
}
