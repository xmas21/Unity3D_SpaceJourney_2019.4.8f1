using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Scenemanager 需要此API
using System.Collections;           // 協程需要此API

#region 欄位
public class MenuManager : MonoBehaviour
{
    [Header("載入畫面")]
    public GameObject panelLoading;  // 整組的就用 GameObject
    [Header("進度")]
    public Text textLoading;         // 文字訊息用 Text
    [Header("進度條")]
    public Image imgLoading;         // 圖像使用 Image
    [Header("載入畫面")]
    public string nameScene = "遊戲場景";
    [Header("提示文字")]
    public GameObject tip;


    #endregion

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(Loading());
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }


    private IEnumerator Loading()
    {
        panelLoading.SetActive(true);                               // 顯示載入畫面
        AsyncOperation ao = SceneManager.LoadSceneAsync(nameScene); // 同步載入場景(場景名稱)
        ao.allowSceneActivation = false;                            // 不要自動載入場景

        // 當 尚未載入場景
        while (!ao.isDone)
        {
            textLoading.text = (ao.progress/0.9f * 100).ToString("F1") + "%";   // 更新文字
            imgLoading.fillAmount = ao.progress / 0.9f;                         // 更新進度條
            yield return null;                                                  // 延遲一個影格時間

            if (ao.progress == 0.9f)
            {
                tip.SetActive(true);

                if (Input.anyKeyDown) ao.allowSceneActivation = true;
            }
        }
    }

}
