using UnityEngine;

public class CustomUnitRotate : MonoBehaviour
{
    #region Show Inpector

    [SerializeField, Tooltip("팀 오브젝트")]
    private Transform teamObject;

    #endregion

    #region Hide Inspector

    //오브젝트의 회전 권한에 대한 변수
    private bool _isRotate;

    #endregion

    private void Update() =>
        teamObjectToRotate();

    //팀 오브젝트를 회전 시킵니다.
    private void teamObjectToRotate()
    {
        if (_isRotate)
            teamObject.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * -10, 0));
        else
        {
            //원래 방향으로 초기화
            var targetPos = Vector3.zero - teamObject.rotation.eulerAngles;
            teamObject.Rotate(targetPos * Time.deltaTime);
        }
    }

    public void ActiveRotate(bool value) => //오브젝트의 회전을 활성화, 비활성화 합니다.
        _isRotate = value;
}
