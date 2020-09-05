using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    #region 欄位

    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話區域")]
    public GameObject paneldialog;
    [Header("說話者")]
    public Text textName;
    [Header("說話內容")]
    public Text textContent;
    [Header("打字速度"), Range(0.1f, 1)]
    public float printSpeed = 0.2f;
    [Header("打字音效")]
    public AudioClip soundPrint;
    [Header("任務區域")]
    public RectTransform panelMission;
    [Header("任務數量")]
    public Text textMission;
    [Header("傳送門")]
    public GameObject[] doors;

    public int count;

    private AudioSource aud;
    private Animator ani;
    private Player player;

    #endregion

    #region 事件
    /// <summary>
    /// 更新任務文字介面
    /// </summary>
    public void UpdateTextMission()
    {
        count++;
        textMission.text ="骷髏頭數量 " + count + "/ " + data.count;
    }

    /// <summary>
    /// 對話系統
    /// </summary>
    public void dialog()
    {
        paneldialog.SetActive(true);
        textName.text = name;
        StartCoroutine(Print());
    }

    /// <summary>
    /// 取消對話
    /// </summary>
    private void CancelDialog()
    {
        paneldialog.SetActive(false);
        ani.SetBool("說話觸發", false);
    }

    /// <summary>
    /// 打字效果
    /// </summary>
    private IEnumerator Print()
    {
        AnimationControl();

        Missioning();

        player.stop = true;

        string dialog = data.dialogs[(int)data._NPCState];                  // 對著 = NPC 資料.對話第一段

        textContent.text = "";                                              // 清空

        for (int i = 0; i < dialog.Length; i++)                             // 跑對話第一個字到最後一個字
        {
            textContent.text += dialog[i];                                  // 對話內容.文字 += 對話[]
            yield return new WaitForSeconds(printSpeed);
        }

        player.stop = false;

        Nomission();
    }

    /// <summary>
    /// 顯示任務
    /// </summary>
    private IEnumerator ShowMission()
    {
        while (panelMission.anchoredPosition.x > 0)
        {
            panelMission.anchoredPosition = Vector3.Lerp(panelMission.anchoredPosition, new Vector3(0, -1.8f, 0), 10 * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 動畫觸發(說話)
    /// </summary>
    private void AnimationControl()
    {
        if (data._NPCState == NPCState.NoMission || data._NPCState == NPCState.Missioning)
            ani.SetBool("說話觸發", true);
        else
            ani.SetTrigger("謝謝觸發");
    }

    /// <summary>
    /// 沒任務階段
    /// </summary>
    private void Nomission()
    {
        if (data._NPCState == NPCState.NoMission)
        {
            data._NPCState = NPCState.Missioning;

            StartCoroutine(ShowMission());
        }
    }

    /// <summary>
    /// 任務中階段
    /// </summary>
    private void Missioning()
    {
        if (count >= data.count) data._NPCState = NPCState.Finish;

        for (int i = 0; i < doors.Length; i++) doors[i].SetActive(true);
    }
    #endregion

    #region 方法

    private void Awake()
    {
        data._NPCState = NPCState.NoMission;

        aud = GetComponent<AudioSource>();

        ani = GetComponent<Animator>();

        player = FindObjectOfType<Player>();              // 透過類型尋找物件*只能有一種* 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "阿兜") CancelDialog();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "阿兜")dialog();     
    }
    #endregion
}
