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

    [Space, SerializeField]
    private RenderTexture renderTexture;

    [SerializeField]
    private GameObject _modelObject;

    [SerializeField]
    private RawImage _rawimage;

    private string _path;

    private void Start()
    {
        _path = $"{Application.persistentDataPath}";
        LogMessage.Log(_path);
    }

    public void UpdateTexture(ref RawImage rawImage,in int[] equipedItems)
    {
        if (null == equipedItems) { return; }

        //_rawimage = rawImage;

        StartCoroutine(ILoadTexture(equipedItems, rawImage));
    }

    public void SaveTexture(in int[] equipedItems)
    {
        StartCoroutine(ISaveTexture(equipedItems));
    }

    private IEnumerator ISaveTexture(int[] equipedItems)
    {
        Util.SaveRenderTextuerToPng(
            $@"{_path}/{equipedItems[0].ToString()}-head,{equipedItems[1].ToString()}-body,{equipedItems[2].ToString()}-leftWeapon,{equipedItems[3].ToString()}-rightWeapon.png",
                    renderTexture);
        
        //중간 텀
        yield return SideTime;
    }

    private IEnumerator ILoadTexture(int[] equipedItems,RawImage rawImage)
    {
        using (var uwr = UnityWebRequestTexture.GetTexture
            ($@"{_path}/{equipedItems[0].ToString()}-head,{equipedItems[1].ToString()}-body,{equipedItems[2].ToString()}-leftWeapon,{equipedItems[3].ToString()}-rightWeapon.png"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                UnitModelManager.Update(_modelObject, equipedItems);

                yield return SideTime;

                Util.SaveRenderTextuerToPng(
                    $@"{_path}/{equipedItems[0].ToString()}-head,{equipedItems[1].ToString()}-body,{equipedItems[2].ToString()}-leftWeapon,{equipedItems[3].ToString()}-rightWeapon.png",
                            renderTexture);

                yield return SideTime;
            }

            var texture = DownloadHandlerTexture.GetContent(uwr);

            rawImage.texture = texture;
        }
    }

    private readonly WaitForSeconds SideTime = new WaitForSeconds(0.2f);
}
