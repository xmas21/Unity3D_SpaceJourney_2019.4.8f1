using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("目標")]
    public Transform target;  
    [Header("速度"),Range(0,100)]         // 攝影機追蹤速度
    public float speed = 1 ;
    [Header("旋轉速度"), Range(0, 10)]    // 旋轉速度
    public float turn = 1;
    [Header("上下角度限制")]
    public Vector2 limit = new Vector2(-30, 30);

    private Quaternion rot;               // 旋轉角度

    /// <summary>
    /// 攝影機追蹤
    /// </summary>
    public void Track()
    {
        Vector3 posA = transform.position;
        Vector3 posB = target.position;
        posA = Vector3.Lerp(posA, posB, Time.deltaTime * speed);
        transform.position = posA;

        if (Input.GetMouseButton(1))
        {
        rot.x += Input.GetAxis("Mouse Y") *turn ;    // 取得滑鼠上下來控制 X 角度
        rot.y += Input.GetAxis("Mouse X") * turn ;   // 取得滑鼠左右來控制 Y 角度

        rot.x = Mathf.Clamp(rot.x, limit.x, limit.y);

        transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void LateUpdate()
    {       
        Track();
    }
}
