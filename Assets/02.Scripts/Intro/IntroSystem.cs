using GameplayIngredients;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroSystem : MonoBehaviour
{
    private UInputSystem uis;

    private void Awake()
    {
        uis = new UInputSystem();
        uis.System.any.performed += AnyOnperformed;
    }
    
    private void AnyOnperformed(InputAction.CallbackContext obj) => 
        Manager.Get<SceneManagerPro>().LoadScene(SceneManagerPro.eScene.LOBBY);

    private void OnEnable() => uis.Enable();

    private void OnDisable() => uis.Disable();
}
