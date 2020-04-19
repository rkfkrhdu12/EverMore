using UnityEngine;
using System.IO;
using System.Linq;

// 여러 기능을 하는 유틸 클래스
public static class Util
{
    public static void SaveRenderTextuerToPng(string path,  RenderTexture rt)
    {
        //렌더 텍스쳐를 바이트화 시킵니다.
        var bytes = toTexture2D(rt).EncodeToPNG();
        
        File.WriteAllBytes(path, bytes);
    }
            
    private static Texture2D toTexture2D(RenderTexture rTex)
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
    
    
    // PNG 불러오기
    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;

        if (File.Exists(filePath))
        {
            var fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            tex.LoadImage(fileData);
        }

        return tex;
    }

    // 스프라이트 배열 불러오기
    public static Sprite[] LoadSprites(string path) =>
        Resources.LoadAll<Sprite>(path);

    // 스프라이트를 PNG 로 변환하기
    public static void SpritesToPng(string path)
    {
        var sprites = LoadSprites(path);
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite spr = sprites[i];
            
            Texture2D tex = new Texture2D((int) spr.rect.width, (int) spr.rect.height, TextureFormat.ARGB32, false);
            // rect : 이미지의 시작위치와 크기

            tex.SetPixels(0, 0, (int) spr.rect.width, (int) spr.rect.height,
                spr.texture.GetPixels((int) spr.rect.x, (int) spr.rect.y, (int) spr.rect.width, (int) spr.rect.height));
            
            //새로 만든 텍스쳐에서 0,0 부터 시작하여 그림
            tex.Apply();
            var bytearray = tex.EncodeToPNG();
            var savepath = $"{Application.dataPath}/Resources/Font/{i:00}.png";
            File.WriteAllBytes(savepath, bytearray);
            Debug.Log(savepath);
        }
    }

    // Random.Range 를 사용할 때, 최댓값의 - 1 까지만 적용되기 때문에,
    // 편하게 사용 할 수 있도록 만든 함수
    public static int Rand(int min, int max) 
        =>  Random.Range(min, max + 1);

    // float 형
    public static float Rand(float min, float max) => 
        Random.Range(min, max);

    // 아이템 확률 배열을 받아와, 그 중 하나를 랜덤으로 받아옴
    // 반드시 매개 변수 priorities 의 원소의 합은 100이 되야함
    public static int GetPriority(int[] priorities)
    {
        int sum = priorities.Sum();

        if (sum <= 0)
            return 0;

        int num = Rand(1, sum); // 1 ~ sum 까지 랜덤으로 받아옴

        sum = 0;
        // 원소를 더하면서 num 이 start 보다 크고 sum 보다 작거나 같을 경우
        // ex) priorities = { 20, 20, 20, 20, 20 }
        // num = 45, start = 40, sum = 60, i = 2
        // 반환 값 2
        for (int i = 0; i < priorities.Length; ++i)
        {
            int start = sum;
            sum += priorities[i];
            
            if (start < num && num <= sum)
                return i;
        }

        return 0;
    }
}
