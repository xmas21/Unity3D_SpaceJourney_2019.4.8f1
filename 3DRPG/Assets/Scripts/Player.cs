using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("速度")]
    public float speed = 1;

    private float attack;
    private float hp;
    private float mp;
    private float exp;
    private int lv;

    private Rigidbody rig;
    private Animator ani;

    #endregion

    #region 事件

    private void FixedUpdate()
    {
        Move();
    }

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    #endregion 

    #region 方法
    /// <summary>
    /// 移動方法 : 前後左右與動畫
    /// </summary>
    private void Move()
    {
        float v = Input.GetAxis("Vertical");                           // 前後 : WS 上下 
        float h = Input.GetAxis("Horizontal");                         // 前後 : AD 左右
        Vector3 pos = transform.forward * v + transform.right * h ;    // 移動座標 = 角色.前方 * 前後 + 角色.右方 * 左右
        rig.MovePosition(transform.position + pos * speed);            // 移動座標 = 角色座標 + 移動座標

        ani.SetFloat("移動", Mathf.Abs(v) + Mathf.Abs(h));             // 移動動畫(取絕對值)

    }

    private void Attack()
    {
        
    }

    private void Skill()
    {
        
    }

    private void GerProp()
    {
        
    }

    private void Hit()
    {
        
    }

    private void Dead()
    {
        
    }

    private void Exp()
    {
        
    }

    #endregion

}
