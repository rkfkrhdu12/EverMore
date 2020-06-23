using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class textPngToBase64 : MonoBehaviour
{
    public RenderTexture rt;

    [Multiline]
    public string myImage;

    public RawImage Profile;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveTexture();

        if (Input.GetKeyDown(KeyCode.A))
            Profile.texture = base64ToTextuer2d(myImage);
    }

    public void SaveTexture()
    {
        //렌더 텍스쳐를 바이트화 시킵니다.
        var bytes = toTexture2D(rt).EncodeToPNG();

        //바이트를 웹에 올리기 좋은 Base64형태로 변환합니다.
        var PngToBase64 = Convert.ToBase64String(bytes);

        Debug.Log(Application.dataPath);
        File.WriteAllBytes(Application.dataPath+"/a.png", bytes);
        //테스트 삼아 다시 텍스쳐 2D로 변환합니다.
        Profile.texture = base64ToTextuer2d(myImage);
    }

    private Texture2D toTexture2D(RenderTexture rTex)
    {
        //렌더 텍스쳐의 가로 X 세로 사이즈로, RGBA32 포맷을 가진 텍스쳐2D를 만듭니다.
        var tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;

        //렌더 텍스쳐 가로 X 세로 사이즈로 읽을 수 있는 픽셀을 처리합니다.
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);

        //적용
        tex.Apply();
        return tex;
    }

    public static Texture2D base64ToTextuer2d(string basePng)
    {
        //임시용으로 Textuer2D를 만듭니다.
        var tex = new Texture2D(1, 1);
        
        //Base64를 바이트로 변환합니다.
        var dae = Convert.FromBase64String(basePng);
        
        //바이트를 이미지로 변환합니다.
        tex.LoadImage(dae);
        return tex;
    }
}
