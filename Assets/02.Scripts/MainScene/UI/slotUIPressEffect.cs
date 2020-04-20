using UnityEngine;

public class slotUIPressEffect : MonoBehaviour
{
    private readonly Vector3 pressScail = new Vector3(.84f, .84f, .84f);

    public void PressEffect() => 
        transform.localScale = pressScail;

    public void PressUpEffect() =>
        transform.localScale = Vector3.one;
}
