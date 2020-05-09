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

    [SerializeField]
    private RawImage _rawimage = null;

    private string _path;

    private void Start()
    {
        _path = $"{Application.persistentDataPath}";
        Debug.Log(_path);
    }

    public void UpdateTexture(ref RawImage rawImage,in int[] equipedItems)
    {
        if (null == equipedItems) { return; }

        partNameToObj.HeadName = equipedItems[0].ToString();
        partNameToObj.BodyName = equipedItems[1].ToString();
        partNameToObj.LeftWeaponName = equipedItems[2].ToString();
        partNameToObj.RightWeaponName = equipedItems[3].ToString();
        _rawimage = rawImage;

        Debug.Log("ILoadTexture : " + equipedItems[0] + " " + equipedItems[1]);

        StartCoroutine(ILoadTexture());
    }

    public void SaveTexture(in int[] equipedItems)
    {
        StartCoroutine(ISaveTexture(equipedItems));
    }

    bool _isComplete = false;

    private IEnumerator ISaveTexture(int[] equipedItems)
    {
        Debug.Log("ISaveTexture : " + equipedItems[0] + " " + equipedItems[1]);

        Util.SaveRenderTextuerToPng(
                    $"{_path}/" +
                    $"{equipedItems[0].ToString()}-head,{equipedItems[1].ToString()}-body," +
                    $"{equipedItems[2].ToString()}-leftWeapon,{equipedItems[3].ToString()}-rightWeapon.png",
                    renderTexture);
        
        //중간 텀
        yield return SideTime;

        _isComplete = true;
    }


    private IEnumerator ILoadTexture()
    {
        using (var uwr = UnityWebRequestTexture.GetTexture($@"{Application.persistentDataPath}/{partNameToObj.HeadName}-head,{partNameToObj.BodyName}-body,{partNameToObj.LeftWeaponName}-leftWeapon,{partNameToObj.RightWeaponName}-rightWeapon.png"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                while (true)
                {
                    if (_isComplete)
                    {
                        _isComplete = false;

                    }
                    yield return SideTime;
                }
            }

            var texture = DownloadHandlerTexture.GetContent(uwr);

            _rawimage.texture = texture;

            //if (uwr.isNetworkError || uwr.isHttpError)
            //{
            //    Util.SaveRenderTextuerToPng(
            //        $"{_path}/{partNameToObj.HeadName}-head,{partNameToObj.BodyName}-body,{partNameToObj.LeftWeaponName}-leftWeapon,{partNameToObj.RightWeaponName}-rightWeapon.png",
            //        renderTexture);

            //    //중간 텀
            //    yield return SideTime;
            //    //StartCoroutine(Iphoto(_path));
            //}

            //var texture = DownloadHandlerTexture.GetContent(uwr);

            //_rawimage.texture = texture;
        }
    }

    private readonly WaitForSeconds SideTime = new WaitForSeconds(0.1f);
}
