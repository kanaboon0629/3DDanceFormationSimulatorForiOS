using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogReceiver : MonoBehaviour
{
    private string url = "";
    public Text logText; // Text コンポーネントの参照
    public Slider progressBar; // 進行状況バーのUIオブジェクト

    private string logBuffer = "";
    private const string SuccessMessage = "Generating demo successful!";
    private const float MaxDuration = 600f; // 10分間    
    private int tabCount = 0;
    // フィルタリング対象のメッセージリスト
    private readonly string[] validMessages = new string[]
    {
        "Getting available formats for the video...",
        "Downloading video from YouTube...",
        "Extracting subclip...",
        "Changing speed of the video...",
        "Generating 2D pose...",
        "Generating 3D pose...",
        "Generating demo...",
        "Generating demo successful!"
    };

    void Start()
    {
        if (PlayerPrefs.HasKey("IPAddress"))
        {
            url = "http://" + PlayerPrefs.GetString("IPAddress") + ":5000";
        }
    }
    public void LogReceiveStart()
    {
        //戻るボタンからの時はやらない
        if (!SceneSwitcher.IsReturningFromNumberSetting)
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

        while (RunPythonScript.IsCommunicatingWithServer)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url + "/log"))
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
                        // ログがフィルタ対象メッセージのいずれかを含んでいるか確認
                        foreach (string validMessage in validMessages)
                        {
                            if (log.Contains(validMessage))
                            {
                                logBuffer = log; // 新しいログをバッファに追加
                                logText.text = logBuffer;
                                UpdateProgressBar();

                                // 成功メッセージが含まれているか確認
                                if (logBuffer.Contains(SuccessMessage))
                                {
                                    Debug.Log("Success message detected. Stopping log polling.");
                                    yield break; // コルーチンを終了
                                }
                                break; // 1つメッセージを処理したらループを抜ける
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.5f); // 0.5秒ごとにポーリング
        }
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
