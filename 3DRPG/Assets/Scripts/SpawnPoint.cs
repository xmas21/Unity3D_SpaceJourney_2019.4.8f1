using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("怪物")]
    public Transform enemy;
    [Header("怪物生成點")]
    public GameObject[] points;
    [Header("怪物生成速度"), Range(0, 5f)]
    public float interval = 2f;

    public NPC npc;

    private void Awake()
    {
        npc = GameObject.Find("Unity醬").GetComponent<NPC>();
    }

    private void Start()
    {
        points = GameObject.FindGameObjectsWithTag("生成點");  // 透過標籤找物件

        InvokeRepeating("Spawn", 1.5f, interval);                 // 重複呼叫("方法名稱",延遲時間,重複頻率)
    }

    private void Spawn()
    {
        if (npc.missionComplete) return;

        int r = Random.Range(0, points.Length);                // 隨機 
        Transform point = points[r].transform;                 // 儲存生成點
        Instantiate(enemy, point.position, point.rotation);      // 生成(物件,座標,角度)
    }
}
