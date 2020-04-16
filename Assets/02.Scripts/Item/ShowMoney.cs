using GameplayIngredients;
using TMPro;
using UnityEngine;

public class ShowMoney : MonoBehaviour
{
    public TMP_Text MoneyText;

    private void Start()
    {
        if (MoneyText != null)
            MoneyText.text = $"{Manager.Get<GameManager>().money:##,###}";
    }

    /// <summary>
    /// 돈의 데이터가 변경되어 화면을 다시 렌더링 해야되는 경우 사용
    /// </summary>
    public void notifyDataMoneyChanged()
    {
        if (MoneyText != null)
            MoneyText.text = $"{Manager.Get<GameManager>().money:##,###}";
    }
}
