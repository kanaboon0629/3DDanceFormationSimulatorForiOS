using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogReceiver : MonoBehaviour
{
    private const string LogUrl = "http://192.168.1.4:5000/log";
    public Text logText; // Text コンポーネントの参照
    public Slider progressBar; // 進行状況バーのUIオブジェクト

    private string logBuffer = "";
    private const string SuccessMessage = "Generating demo successful!";
    private const float MaxDuration = 600f; // 10分間    
    private int tabCount = 0;

    void Start()
    {
        //戻るボタンからの時はやらない
        if (!SceneSwitcher.IsReturningFromSpecificScene)
        {
            tabCount = PlayerPrefs.GetInt("tabCount");
            StartCoroutine(StartLogStreamAfterDelay());
        }
    }

    IEnumerator StartLogStreamAfterDelay()
    {
        // 3秒待機
        yield return new WaitForSeconds(3f);

        // GetLogStreamコルーチンを開始
        StartCoroutine(GetLogStream());
    }

    IEnumerator GetLogStream()
    {
        float startTime = Time.time; // コルーチンの開始時刻を記録

        // 10分以内かつ成功メッセージが出るまでループ
        while (Time.time - startTime < MaxDuration)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(LogUrl))
            {
                webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    yield return null; // フレーム毎に待機
                }

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    string log = webRequest.downloadHandler.text;
                    if (!string.IsNullOrEmpty(log))
                    {
                        logBuffer = log; // 新しいログをバッファに追加
                        logText.text = logBuffer;

                        UpdateProgressBar();

                        if (logBuffer.Contains(SuccessMessage))
                        {
                            Debug.Log("Success message detected. Stopping log polling.");
                            yield break; // コルーチンを終了
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.5f); // 1秒ごとにポーリング
        }

        Debug.Log("Polling stopped after 10 minutes.");
    }

    private void UpdateProgressBar()
    {
        if (progressBar == null) return;

        if (tabCount == 0)
        {
            // タブカウントが0のときのログメッセージ
            if (logBuffer.Contains("Generating 2D pose..."))
            {
                progressBar.value = 0.25f; // 25% 完了
            }
            else if (logBuffer.Contains("Generating 3D pose..."))
            {
                progressBar.value = 0.5f; // 50% 完了
            }
            else if (logBuffer.Contains("Generating demo..."))
            {
                progressBar.value = 0.75f; // 75% 完了
            }
            else if (logBuffer.Contains(SuccessMessage))
            {
                progressBar.value = 1.0f; // 100% 完了
            }
        }
        else if (tabCount == 1)
        {
            // タブカウントが1のときのログメッセージ
            if (logBuffer.Contains("Getting available formats for the video..."))
            {
                progressBar.value = 0.05f; // 5% 完了
            }
            else if (logBuffer.Contains("Downloading video from YouTube..."))
            {
                progressBar.value = 0.1f; // 10% 完了
            }
            else if (logBuffer.Contains("Extracting subclip..."))
            {
                progressBar.value = 0.15f; // 15% 完了
            }
            else if (logBuffer.Contains("Changing speed of the video..."))
            {
                progressBar.value = 0.2f; // 20% 完了
            }
            else if (logBuffer.Contains("Generating 2D pose..."))
            {
                progressBar.value = 0.4f; // 40% 完了
            }
            else if (logBuffer.Contains("Generating 3D pose..."))
            {
                progressBar.value = 0.6f; // 60% 完了
            }
            else if (logBuffer.Contains("Generating demo..."))
            {
                progressBar.value = 0.8f; // 80% 完了
            }
            else if (logBuffer.Contains(SuccessMessage))
            {
                progressBar.value = 1.0f; // 100% 完了
            }
        }
    }
}
