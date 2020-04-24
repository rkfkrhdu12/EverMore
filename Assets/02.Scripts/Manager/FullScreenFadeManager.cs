using System;
using System.Collections;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ManagerDefaultPrefab("FullScreenFadeManager")]
public class FullScreenFadeManager : Manager
{
    public enum FadeMode
    {
        FadeOut = 0,
        FadeIn = 1
    }

    public Image FullScreenFadePlane;

    private Coroutine m_Coroutine;

    public UnityEvent onComplete;

    /// <summary>
    /// 페이드 연출을 처리합니다.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="mode">ToBlack을 사용시 페이드 인되며, FadeOut</param>
    /// <param name="OnComplete"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Fade(float duration, FadeMode mode, Color Fadecolor, UnityAction OnComplete = null)
    {
        //지속시간이 0미만일 순 없습니다.
        if (duration < 0f)
            return;

        //이미 재생중인 페이드 아웃이 있다면 종료합니다.
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }

        //색상 변경
        var color = FullScreenFadePlane.color;
        color.r = Fadecolor.r;
        color.g = Fadecolor.g;
        color.b = Fadecolor.b;

        //알파 적용
        switch (mode)
        {
            case FadeMode.FadeIn:
                color.a = 0.0f;
                break;
            case FadeMode.FadeOut:
                color.a = 1.0f;
                break;
        }

        //초기 알파 적용
        FullScreenFadePlane.color = color;
        m_Coroutine = StartCoroutine(FadeCoroutine(duration, color.a, mode == FadeMode.FadeOut ? 0.0f : 1.0f));
    }

    private IEnumerator FadeCoroutine(float duration, float initAlpha, float target)
    {
        //페이드가 화면에 보여야하므로 활성화 해준다.
        FullScreenFadePlane.gameObject.SetActive(true);

        //초기 값을 잡아준다.
        var PlaneColor = FullScreenFadePlane.color;

        //시간
        var time = 0.0f;

        //알파값이 target값이 아니면 계속 반복한다.
        while (!Mathf.Approximately(PlaneColor.a, target))
        {
            time += Time.deltaTime / duration;
            PlaneColor.a = Mathf.Lerp(initAlpha, target, time);
            FullScreenFadePlane.color = PlaneColor;
            yield return null;
        }

        //오브젝트를 비활성화 함
        FullScreenFadePlane.gameObject.SetActive(false);

        //끝 마무리에 발생해야되는 이벤트가 있을 시, 실행
        onComplete?.Invoke();

        //초기화
        m_Coroutine = null;
        onComplete = null;
    }
}
