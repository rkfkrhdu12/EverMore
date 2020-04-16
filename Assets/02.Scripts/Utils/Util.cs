using UnityEngine;
using System.IO;
public class Util {
    // 여러 기능을 하는 유틸 클래스
    public static Texture2D LoadPNG(string filePath)
        // PNG 불러오기
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            tex.LoadImage(fileData); 
        }
        return tex;
    }
	public static Sprite[] LoadSprites(string path)
        // 스프라이트 배열 불러오기
	{
		return Resources.LoadAll<Sprite>(path);
	}
	public static void SpritesToPng(string path)
        // 스프라이트를 PNG 로 변환하기
	{
		var sprites = LoadSprites(path);
		for (int i = 0; i < sprites.Length; i++)
		{
			
			Sprite spr = sprites[i];
			Texture2D tex = new Texture2D((int)spr.rect.width, (int)spr.rect.height, TextureFormat.ARGB32, false);
			// rect : 이미지의 시작위치와 크기

			tex.SetPixels(0, 0, (int)spr.rect.width, (int)spr.rect.height, spr.texture.GetPixels((int)spr.rect.x, (int)spr.rect.y, (int)spr.rect.width, (int)spr.rect.height));
			//새로 만든 텍스쳐에서 0,0 부터 시작하여 그림
			tex.Apply();
			var bytearray = tex.EncodeToPNG();
			var savepath = string.Format("{0}/Resources/Font/{1:00}.png", Application.dataPath, i);
			File.WriteAllBytes(savepath, bytearray);
			Debug.Log(savepath);
		}
	}
    public static int Rand(int min, int max)
        // Random.Range 를 사용할 때, 최댓값의 - 1 까지만 적용되기 때문에,
        // 편하게 사용 할 수 있도록 만든 함수
    {
        return Random.Range(min, max + 1);
    }

    public static float Rand(float min, float max)
        // float 형
    {
        return Random.Range(min, max);
    }
    public static int GetPriority(int[] priorities)
        // 아이템 확률 배열을 받아와, 그 중 하나를 랜덤으로 받아옴
        // 반드시 매개 변수 priorities 의 원소의 합은 100이 되야함
    {
        int sum = 0;
        for (int i = 0; i < priorities.Length; ++i)
			//전부 합산
        {
            sum += priorities[i];
        }

        if (sum <= 0)
            return 0;

        int num = Rand(1, sum); // 1 ~ sum 까지 랜덤으로 받아옴

        sum = 0;
        for (int i = 0; i < priorities.Length; ++i)
            // 원소를 더하면서 num 이 start 보다 크고 sum 보다 작거나 같을 경우
            // ex) priorities = { 20, 20, 20, 20, 20 }
            // num = 45, start = 40, sum = 60, i = 2
            // 반환 값 2
        {
            int start = sum;
            sum += priorities[i];
            if (start < num && num <= sum)
            {
                return i;
            }
        }

        return 0;
    }
}
