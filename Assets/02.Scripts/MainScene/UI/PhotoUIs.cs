using System;
using GameplayIngredients;
using UnityEngine;
using UnityEngine.UI;

public class PhotoUIs : MonoBehaviour
{
    [SerializeField]
    private RawImage[] Photo;

    private void Awake()
    {
        var unit = Manager.Get<GameManager>().getPlayerUnitCount();
        Debug.Log(unit);
    }

    private void OnEnable()
    {
   
    }
}
