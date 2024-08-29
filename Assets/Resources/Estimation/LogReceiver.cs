using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogReceiver : MonoBehaviour
{
    private const string LogUrl = "http://192.168.1.4:5000//log";
    public Text logText; // Text コンポーネントの参照
    private string logBuffer = "";
    private const string SuccessMessage = "Generating demo successful!";
    private const float MaxDuration = 600f; // 10分間

    void Start()
    {
        if (logText == null)
        {
            Debug.LogError("LogText is not assigned!");
            return;
        }

        StartCoroutine(StartLogStreamAfterDelay());
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

                        if (logBuffer.Contains(SuccessMessage))
                        {
                            Debug.Log("Success message detected. Stopping log polling.");
                            yield break; // コルーチンを終了
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1.0f); // 1秒ごとにポーリング
        }

        Debug.Log("Polling stopped after 10 minutes.");
    }
}