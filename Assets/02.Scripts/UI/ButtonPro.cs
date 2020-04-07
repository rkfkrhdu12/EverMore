using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPro : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
    IPointerUpHandler
{
    #region Enum

    private enum BtnState
    {
        NORMAL,
        PRESS
    }
    
    #endregion

    #region Show Inspector

    [Header("이미지")]
    [SerializeField, Tooltip("기본 상태일 때")]
    private Sprite normalImage;

    [SerializeField, Tooltip("마우스가 닿았을 때")]
    private Sprite reachImage;

    [SerializeField, Tooltip("눌렸을 때")]
    private Sprite pressImage;
    
    [SerializeField, Tooltip("누르고 땟을 때, 함수를 실행합니다.")]
    private UnityEvent onDownUp;
    
    #endregion

    #region Hide Inspector

    //이미지 변수
    private Image image;

    //버튼의 상태에 대한 변수
    private BtnState _btnState;
    
    //그룹화로 만들시, 선택권이 있는 버튼
    [HideInInspector]
    public bool isSelected;
    
    #endregion

    private void Awake() => //초기화 해줍니다.
        image = GetComponent<Image>();

    //버튼을 누름
    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = pressImage;
        _btnState = BtnState.PRESS;
    }

    //버튼을 땜
    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = normalImage;
        _btnState = BtnState.NORMAL;
        onDownUp?.Invoke();
    }

    //버튼 영역에 닿음
    public void OnPointerEnter(PointerEventData eventData)
        => image.sprite = reachImage;

    //버튼 영역에서 나감
    public void OnPointerExit(PointerEventData eventData)
    {
        //누르고 있는 상태라면, 아래 코드 구문 실행 X
        if (_btnState == BtnState.PRESS) return;

        //마우스가 UI영역 밖으로 나갔으므로, 초기화
        image.sprite = normalImage;
        _btnState = BtnState.NORMAL;
    }
}
