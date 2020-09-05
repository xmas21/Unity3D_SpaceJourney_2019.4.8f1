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
    [Header("怪物的經驗值"), Range(30, 100)]
    public float exp = 30;
    [Header("攻擊停止距離"), Range(0.1f, 3)]
    public float distanceAttack = 2.5f;

    private Transform player;
    private NavMeshAgent nav;
    private Animator ani;




    #endregion

    #region 方法

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        nav.speed = speed;
        nav.stoppingDistance = distanceAttack;

        player = GameObject.Find("阿兜").transform;


    }

    private void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawSphere(transform.position, distanceAttack);
    }

    private void Move()
    {
        nav.SetDestination(player.position);
        ani.SetFloat("移動", nav.velocity.magnitude);

        if (nav.remainingDistance < distanceAttack) Attack();
    }

    private void Attack()
    {
        ani.SetTrigger("攻擊觸發");
    }

    #endregion

    #region 事件







    #endregion
}
