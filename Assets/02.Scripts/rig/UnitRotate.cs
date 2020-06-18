using UnityEngine;
using UnityEngine.EventSystems;

public class UnitRotate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Show Inspector

    [SerializeField, Tooltip("오브젝트 회전 속도")]
    private float rotSpeed = 1f;

    [SerializeField, Tooltip("실제 유닛 오브젝트")]
    private Transform Units = null;

    [SerializeField, Tooltip("원래 방향으로 되돌아오는 속도")]
    private float OriginalTurnSpeed = 1f;
    
    #endregion

    #region Hide Inspector

    //회전을 허락할지 말지의 변수
    private bool isRotate;

    #endregion
    
    
    private void Update()
    {
        //유닛 미리보기를 마우스 놓을시, 다시 되돌아오게 하는 로직
        RotatedBackToOriginalDirection();

        //유닛 미리보기를 클릭 시, 회전 가능하게 하는 로직
        UnitRotation();
    }

    private void RotatedBackToOriginalDirection()
    {
        if(!isRotate)
            Units.rotation = Quaternion.Lerp(
                Units.rotation,
                Quaternion.Euler(0,0,0),
                OriginalTurnSpeed *Time.deltaTime);
    }
    
    private void UnitRotation()
    {
        // 회전 권한이 없거나, 마우스가 클릭되지 않았다면 : return
        if (!isRotate || !Input.GetMouseButton(0)) return;
        
        var MouseX = Input.GetAxis("Mouse X");

        // 회전
        Units.Rotate(-Vector3.up * (rotSpeed * MouseX));
    }
    
    public void OnPointerDown(PointerEventData eventData) =>
        isRotate = true;

    public void OnPointerUp(PointerEventData eventData) =>
        isRotate = false;
}
