using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region 欄位

    [Header("移動速度"), Range(0.1f, 3)]
    public float speed = 1.5f;
    [Header("攻擊力"), Range(35f, 50f)]
    public float attack = 40f;
    [Header("血量"), Range(200, 300)]
    public float hp = 200;
    [Header("怪物的經驗值"), Range(30, 100000)]
    public float exp = 30;
    [Header("攻擊停止距離"), Range(0.1f, 3)]
    public float distanceAttack = 2.5f;
    [Header("攻擊冷卻時間"), Range(0.1f, 5)]
    public float cd = 4;
    [Header("轉頭速度"), Range(0.1f, 50)]
    public float turn = 5;
    [Header("骷髏頭")]
    public Transform skull;
    [Header("掉落機率"), Range(0f, 1f)]
    public float skullProp = 0.6f;

    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;
    private float timer;

    private Rigidbody rig;



    #endregion

    #region 方法

    private void Awake()
    {
        player = GameObject.Find("阿兜").transform;

        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        nav.speed = speed;
        nav.stoppingDistance = distanceAttack;

        nav.SetDestination(player.position);
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        float range = Random.Range(-20f, 20f);
        if (other.name == "阿兜") other.GetComponent<Player>().Hit(attack + range, transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, distanceAttack);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "碎石")
        {
            Hit(player.GetComponent<Player>().skillDamage, player.transform);
        }
    }

    private void Move()
    {
        nav.SetDestination(player.position);
        ani.SetFloat("移動", nav.velocity.magnitude);

        if (nav.remainingDistance < distanceAttack) Attack();
    }

    /// <summary>
    /// 怪物攻擊
    /// </summary>
    private void Attack()
    {
        Quaternion look = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * turn);

        timer += Time.deltaTime;

        if (timer >= cd)
        {
            timer = 0;
            ani.SetTrigger("攻擊觸發");
        }
    }

    /// <summary>
    /// 怪物受傷
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="direction"></param>
    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        ani.SetTrigger("受傷觸發");
        rig.AddForce(direction.forward * 100 + direction.up * 100);

        hp = Mathf.Clamp(hp, 0, 999);

        if (hp == 0) Dead();
    }

    /// <summary>
    /// 怪物死亡
    /// </summary>
    private void Dead()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        ani.SetBool("死亡觸發", true);
        enabled = false;
        nav.isStopped = true;
        player.GetComponent<Player>().Exp(exp);


        float r = Random.Range(0f, 1f);

        if (r <= skullProp) Instantiate(skull, transform.position + Vector3.up * 10, transform.rotation);

    }

    #endregion
}
