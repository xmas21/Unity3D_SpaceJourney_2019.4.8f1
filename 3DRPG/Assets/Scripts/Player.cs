using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 欄位
    [Header("速度"), Range(0, 100)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 100)]
    public float turn = 1;
    [Header("傳送門")]
    public Transform[] doors;
    [Header("介面區塊")]
    public Image barHp;
    public Image barMp;
    public Image barExp;
    [Header("流星雨")]
    public Transform stone;
    public Text textLv;
    public float[] exps = new float[99];


    /// <summary>
    /// 停止不移動
    /// </summary>
    [HideInInspector]
    public bool stop;
    private float stoneCost = 10;

    public float attack = 100;
    private float hp = 100;
    private float maxHp = 100;
    private float mp = 100;
    private float maxMp = 100;
    private float restoreMp = 10;
    private float exp;
    private float MaxExp = 100;
    private int lv = 1;

    [HideInInspector]
    public float skillDamage = 200;
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

        for (int i = 0; i < exps.Length; i++) exps[i] = 100 * (i + 1);
    }

    private void Update()
    {
        Attack();
        Skill();
        RestoreMp();
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

        if (other.tag == "石頭怪")
        {
            print("1");
            other.GetComponent<Enemy>().Hit(attack, transform);
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetTrigger("攻擊觸發");
        }
    }

    private void Skill()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (stoneCost <= mp)
            {
                mp -= stoneCost;
                barMp.fillAmount = mp / maxMp;
                Vector3 pos = transform.forward * 2 + transform.up * 5;
                Instantiate(stone, transform.position + pos, transform.rotation);
            }
        }
    }

    private void GetProp(GameObject prop)
    {
        Destroy(prop);
        npc.UpdateTextMission();
    }

    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
        rig.AddForce(direction.forward * 100 + direction.up * 100);

        hp = Mathf.Clamp(hp, 0, 99999);
        barHp.fillAmount = hp / maxHp;

        if (hp == 0) Dead();
    }


    private void Dead()
    {
        ani.SetBool("死亡觸發", true);
        enabled = false;
    }

    public void Exp(float getExp)
    {
        exp += getExp;
        barExp.fillAmount = exp / MaxExp;

        while (exp >= MaxExp) LevelUp();
    }

    private void LevelUp()
    {
        lv++;
        maxHp += 20;
        maxMp += 10;
        attack += 10;
        skillDamage += 10;

        hp = maxHp;
        mp = maxMp;
        exp -= MaxExp;

        MaxExp = exps[lv - 1];

        barHp.fillAmount = 1;
        barMp.fillAmount = 1;
        barExp.fillAmount = exp / MaxExp;
        textLv.text = "Lv" + lv;

    }

    private void RestoreMp()
    {
        mp += restoreMp * Time.deltaTime;
        mp = Mathf.Clamp(mp, 0, maxMp);
        barMp.fillAmount = mp / maxMp;
    }

    #endregion

}
