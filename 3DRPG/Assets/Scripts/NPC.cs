using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話區域")]
    public GameObject paneldialog;
    [Header("說話者")]
    public Text textName;
    [Header("說話內容")]
    public Text textContent;

    public void dialog()
    {
        paneldialog.SetActive(true);
        textName.text = name;
        textContent.text = data.dialogs[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "阿兜")dialog();     
    }
}
