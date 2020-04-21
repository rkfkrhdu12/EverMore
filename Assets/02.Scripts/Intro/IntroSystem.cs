using System;
using GameplayIngredients;
using UnityEngine;

public class IntroSystem : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKeyDown)
            Manager.Get<SceneManagerPro>().LoadScene(SceneManagerPro.eScene.LOBBY);
    }
}
