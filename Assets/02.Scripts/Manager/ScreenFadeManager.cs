using System;
using System.Collections;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ManagerDefaultPrefab("ScreenFadeManager")]
public class ScreenFadeManager : Manager
{
    public enum FadeMode
    {
        FromBlack = 0,
        ToBlack = 1
    }

    public Image FullScreenFadePlane;

    private Coroutine m_Coroutine;

    /// <summary>
    /// 페이드 연출을 처리합니다.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="mode"></param>
    /// <param name="OnComplete"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Fade(float duration, FadeMode mode, Color Fadecolor, UnityAction OnComplete = null)
    {
        var color = FullScreenFadePlane.color;
        color.r = Fadecolor.r;
        color.g = Fadecolor.g;
        color.b = Fadecolor.b;
        FullScreenFadePlane.color = color;
        
        if (m_Coroutine != null)
        {
            StopCoroutine(m_Coroutine);
            m_Coroutine = null;
        }

        if (duration <= 0.0f)
        {
            switch (mode)
            {
                case FadeMode.ToBlack:
                    color.a = 1.0f;
                    break;
                case FadeMode.FromBlack:
                    color.a = 0.0f;
                    break;
                default: throw new NotImplementedException();
            }

            FullScreenFadePlane.gameObject.SetActive(color.a == 1.0f);
            FullScreenFadePlane.color = color;
        }
        else
        {
            switch (mode)
            {
                case FadeMode.ToBlack:
                    m_Coroutine = StartCoroutine(FadeCoroutine(duration, 1.0f, 1.0f));
                    break;
                case FadeMode.FromBlack:
                    m_Coroutine = StartCoroutine(FadeCoroutine(duration, 0.0f, -1.0f));
                    break;
                default: throw new NotImplementedException();
            }
        }
    }

    private IEnumerator FadeCoroutine(float duration, float target, float sign, UnityAction OnComplete = null)
    {
        FullScreenFadePlane.gameObject.SetActive(true);

        while (sign > 0 ? FullScreenFadePlane.color.a <= target : FullScreenFadePlane.color.a >= target)
        {
            var c = FullScreenFadePlane.color;
            c.a += sign * Time.unscaledDeltaTime / duration;
            FullScreenFadePlane.color = c;
            yield return new WaitForEndOfFrame();
        }

        Color finalColor = FullScreenFadePlane.color;
        finalColor.a = target;
        FullScreenFadePlane.color = finalColor;
        OnComplete?.Invoke();
        FullScreenFadePlane.gameObject.SetActive(target != 0.0f);

        yield return new WaitForEndOfFrame();
        m_Coroutine = null;
    }
}
