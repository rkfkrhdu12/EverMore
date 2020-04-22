using System;
using GameplayIngredients;
using GameplayIngredients.Logic;
using UnityEngine;
using UnityEngine.AnimatorPro;
using UnityEngine.UI;

public class IntroSystem : MonoBehaviour
{
    #region Show Inspector

    [SerializeField, Tooltip("타이틀 트랜스폼을 수록해주세요.")]
    private RectTransform title;
    
    [SerializeField, Tooltip("타이틀의 애니메이션을 수록해주세요.")]
    private Animator anim;

    [SerializeField, Tooltip("아무키나 누르라는 텍스트를 수록해주세요")]
    private GameObject GameStartText;

    #endregion

    #region Hide Inspector

    private AnimatorPro animatorPro;

    //아무키나 눌러서 게임을 시작할 수 있게 해주는 변수 입니다.
    private bool isGameStart;

    //라이팅이 다시 비치는 시간을 재기 위한 변수 입니다.
    private float LightRecycleTime;

    private static readonly int ID_Light = Animator.StringToHash("Recycle"); 
    
    #endregion
    
    private void Awake()
    {
        animatorPro = GetComponent<AnimatorPro>();
        animatorPro.Init(anim);
        
        Manager.Get<ScreenFadeManager>().Fade(1.3f,ScreenFadeManager.FadeMode.FromBlack,Color.black);
    }

    private void Update()
    {
        if(isGameStart && Input.anyKeyDown)
            Manager.Get<SceneManagerPro>().LoadScene(SceneManagerPro.eScene.LOBBY);
    }

    [AnimatorExit("Base Layer.MoveUp")]
    public void GameStart()
    {
        isGameStart = true;
        GameStartText.SetActive(true);
    }

    [AnimatorStay("Base Layer.Light")]
    [AnimatorStay("Base Layer.StarRotation")]
    public void ReStartLight()
    {
        LightRecycleTime += Time.deltaTime;

        if (LightRecycleTime > 5f)
        {
            animatorPro.SetTrigger(ID_Light);
            LightRecycleTime = 0f;
        }
    }
}
