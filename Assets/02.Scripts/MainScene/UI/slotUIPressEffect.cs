using UnityEngine;

public class slotUIPressEffect : MonoBehaviour
{
    private readonly Vector3 pressScail = new Vector3(1.148f, 1.148f, 1.148f);

    [SerializeField]
    private Transform textureTrs;

    public void PressEnter()
    {
        //transform.localScale = pressScail;
        textureTrs.localScale = pressScail;
    }

    public void PressExit()
    {
        //transform.localScale = Vector3.one;
        textureTrs.localScale = Vector3.one;
    }
}
