using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("速度"), Range(0, 100)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 100)]
    public float turn = 1;
    public Transform[] doors;

    /// <summary>
    /// 停止不移動
    /// </summary>
    [HideInInspector]
    public bool stop;

    private float attack;
    private float hp;
    private float mp;
    private float exp;
    private int lv;

    private Rigidbody rig;
    private Animator ani;
    private Transform cam;
    private AudioSource aud;
    private NPC npc;

    #endregion

    #region 事件

    /// <summary>
    /// 開局的事
    /// </summary>
    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        cam = GameObject.Find("攝影機根目錄").transform;
        npc = FindObjectOfType<NPC>();
    }

    /// <summary>
    /// 傳送門
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "傳送門 - NPC")
        {
            transform.position = doors[1].position;
            doors[1].GetComponent<CapsuleCollider>().enabled = false;
            Invoke("OpenDoorNPC", 4);

        }

        if (other.name == "傳送門 - BOSS")
        {
            transform.position = doors[0].position;
            doors[0].GetComponent<CapsuleCollider>().enabled = false;
            Invoke("OpenDoorBOSS", 4);
        }
    }

    /// <summary>
    /// 開啟NPC傳送門
    /// </summary>
    private void OpenDoorNPC()
    {
        doors[1].GetComponent<CapsuleCollider>().enabled = true;
    }

    /// <summary>
    /// 開啟BOSS傳送門
    /// </summary>
    private void OpenDoorBOSS()
    {
        doors[0].GetComponent<CapsuleCollider>().enabled = true;
    }

    private void FixedUpdate()
    {
        if (stop) return;  // 如果 停止 跳出

        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "骷髏頭") GetProp(collision.gameObject);
    }

    #endregion 

    #region 方法
    /// <summary>
    /// 移動方法 : 前後左右與動畫
    /// </summary>
    private void Move()
    {
        float v = -Input.GetAxis("Vertical");                   // 前後 : WS 上下 
        float h = -Input.GetAxis("Horizontal");                 // 前後 : AD 左右
        Vector3 pos = cam.forward * v + cam.right * h;          // 移動座標 = 角色.前方 * 前後 + 角色.右方 * 左右
        rig.MovePosition(transform.position + pos * speed);     // 移動座標 = 角色座標 + 移動座標

        ani.SetFloat("移動", Mathf.Abs(v) + Mathf.Abs(h));      // 移動動畫(取絕對值)

        if (v != 0 || h != 0)                                                       // 如果 控制中  
        {
            pos.y = 0;
            Quaternion angle = Quaternion.LookRotation(pos);                        // 角度 = 面向(移動座標)
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, turn); // 角度 = 角度,插值(角度A, 角度B, 旋轉速度)
        }

    }

    private void Attack()
    {

    }

    private void Skill()
    {

    }

    private void GetProp(GameObject prop)
    {
        Destroy(prop);
        npc.UpdateTextMission();
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
