using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;

public class RunPythonScript : MonoBehaviour
{
    public GameObject loadingSpinner; // ローディングスピナーのUIオブジェクト
    public Slider progressBar; // 進行状況バーのUIオブジェクト
    public GameObject nextButton; // JSON作成完了後に表示するボタンのUIオブジェクト
    public GameObject checkButton; // JSON作成完了後に表示するボタンのUIオブジェクト
    public GameObject retryButton;
    private const string SuccessMessage = "Generating demo successful!";
    public static bool IsCommunicatingWithServer = false;
    public LogReceiver logReceiver;

    public Text logText;
    private UnityWebRequest currentRequest; // 現在のリクエストを保持する変数
    private Coroutine currentCoroutine; // 現在のコルーチンを保持する変数

    private IEnumerator Start()
    {
        yield return null;
        //戻るボタンからの時はやらない
        if (!SceneSwitcher.IsReturningFromSpecificScene)
        {
            logText.text = "Communicating with the server...";
            nextButton.SetActive(false); // ボタンを非表示にしておく
            checkButton.SetActive(false); // ボタンを非表示にしておく
            // 最初にサーバーの状態をチェックする
            yield return StartCoroutine(CheckServerStatusCoroutine());
        }
    }
    public void CheckServerStatus()
    {
        StartCoroutine(CheckServerStatusCoroutine());
    }
    private IEnumerator CheckServerStatusCoroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://192.168.1.6:5000/status"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error checking server status: {request.error}");
                logText.text = $"Error checking server status: {request.error}";
                yield break; // 終了する場合はここで処理を終了
            }

            string responseText = request.downloadHandler.text;
            Debug.Log($"Server status: {responseText}");

            if (responseText.Contains("\"status\": \"running\""))
            {
                logText.text = "Server is Used";
                retryButton.SetActive(true);
            }else{
                //推定実行
                Running();
            }
        }
    }
    public void Running()
    {
        logText.text = "Communicating with the server...";
        retryButton.SetActive(false);
        loadingSpinner.SetActive(true);
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0f;
        StartCoroutine(RunningCoroutine());
    }

    private IEnumerator RunningCoroutine()
    {
        string requestId = Guid.NewGuid().ToString(); // 一意のリクエストIDを生成
        PlayerPrefs.SetString("requestId", requestId); // プレファレンスに保存

        int tabCount = PlayerPrefs.GetInt("tabCount");
        if (tabCount == 0)
        {
            string selectedVideoPath = PlayerPrefs.GetString("selectedVideoPath");
            if (string.IsNullOrEmpty(selectedVideoPath))
            {
                Debug.LogError("Video path is not set.");
                logText.text = "Video path is not set.";
                yield break;
            }
            IsCommunicatingWithServer = true; // Start of request
            logReceiver.LogReceiveStart(); // Start the log receiver
            currentCoroutine = StartCoroutine(SendVideo(selectedVideoPath, requestId));
            yield return currentCoroutine;
            IsCommunicatingWithServer = false; // End of request
        }
        else if (tabCount == 1)
        {
            string url = PlayerPrefs.GetString("url");
            string startStr = PlayerPrefs.GetString("start");
            string endStr = PlayerPrefs.GetString("end");
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(startStr) || string.IsNullOrEmpty(endStr))
            {
                Debug.LogError("URL or start/end parameters are missing.");
                logText.text = "URL or start/end parameters are missing.";
                yield break;
            }

            if (!int.TryParse(startStr, out int start) || !int.TryParse(endStr, out int end))
            {
                Debug.LogError("Start or end parameters are not valid integers.");
                logText.text = "Start or end parameters are not valid integers.";
                yield break;
            }
            IsCommunicatingWithServer = true; // Start of request
            logReceiver.LogReceiveStart(); // Start the log receiver
            currentCoroutine = StartCoroutine(SendRequest(url, start, end, requestId));
            yield return currentCoroutine;
            IsCommunicatingWithServer = false; // End of request
        }
        loadingSpinner.SetActive(false);
        progressBar.gameObject.SetActive(false);
    }

    private IEnumerator SendVideo(string filePath, string requestId)
    {
        byte[] videoData = System.IO.File.ReadAllBytes(filePath);

        Debug.Log($"Sending file: {filePath}");

        WWWForm form = new WWWForm();
        form.AddBinaryData("file", videoData, Path.GetFileName(filePath), "video/mp4");
        form.AddField("requestId", requestId); // リクエストIDを追加

        yield return SendRequest("http://192.168.1.6:5000/run-script-from-videofile", form);
    }

    private IEnumerator SendRequest(string url, int start, int end, string requestId)
    {
        var requestData = new RequestData
        {
            url = url,
            start = start,
            end = end,
            requestId = requestId // リクエストIDを追加
        };
        var jsonData = JsonUtility.ToJson(requestData);

        Debug.Log($"Sending JSON data: {jsonData}");

        yield return SendRequest("http://192.168.1.6:5000/run-script-from-youtube", jsonData, "application/json");
    }

    private IEnumerator SendRequest(string endpoint, WWWForm form)
    {
        using (currentRequest = UnityWebRequest.Post(endpoint, form))
        {
            currentRequest.uploadHandler = new UploadHandlerRaw(form.data);
            currentRequest.downloadHandler = new DownloadHandlerBuffer();
            currentRequest.SetRequestHeader("Content-Type", form.headers["Content-Type"]);

            yield return HandleRequestResponse();
        }
    }

    private IEnumerator SendRequest(string endpoint, string jsonData, string contentType)
    {
        using (currentRequest = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            currentRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            currentRequest.downloadHandler = new DownloadHandlerBuffer();
            currentRequest.SetRequestHeader("Content-Type", contentType);

            yield return HandleRequestResponse();
        }
    }

    private IEnumerator HandleRequestResponse()
    {
        yield return currentRequest.SendWebRequest();

        if (currentRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Request Error: {currentRequest.error}");
            logText.text = "Request Error";
            loadingSpinner.SetActive(false);
            progressBar.gameObject.SetActive(false);
        }
        else
        {
            string responseText = currentRequest.downloadHandler.text;

            if (responseText.StartsWith("{") && responseText.EndsWith("}"))
            {
                string jsonFilePath = Path.Combine(Application.persistentDataPath, "output.json");
                File.WriteAllBytes(jsonFilePath, System.Text.Encoding.UTF8.GetBytes(responseText));

                ProcessJsonFile(jsonFilePath);
                CreateSymmetryFile(jsonFilePath);
                Debug.Log("JSON作成完了");
                nextButton.SetActive(true);
                checkButton.SetActive(true);
                logText.text = SuccessMessage;
            }
        }
    }
        public void CancelRequest()
    {
        if (IsCommunicatingWithServer){
            string requestId = PlayerPrefs.GetString("requestId","0");
            StartCoroutine(SendCancelRequestCoroutine(requestId));
            IsCommunicatingWithServer = false;
        }
    }

    private IEnumerator SendCancelRequestCoroutine(string requestId)
    {
        // リクエストIDを含むJSONデータを作成
        string jsonData = "{\"requestId\": \"" + requestId + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // UnityWebRequestの作成
        UnityWebRequest www = new UnityWebRequest("http://192.168.1.6:5000/cancel-request", "POST");
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // リクエストの送信と結果の待機
        yield return www.SendWebRequest();

        // リクエストの結果を処理
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Request cancelled successfully: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error cancelling request: " + www.error);
        }
    }


    private void ProcessJsonFile(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        Debug.Log($"JSON file created at: : {filePath}");
    }

    private void CreateSymmetryFile(string inputFilePath)
    {
        string outputFilePath = inputFilePath.Replace(".json", "Symmetry.json");

        SymmetryJsonProcessor.ProcessJson(inputFilePath, outputFilePath);

        Debug.Log($"Symmetry JSON file created at: {outputFilePath}");
    }

    [System.Serializable]
    public class RequestData
    {
        public string url;
        public int start;
        public int end;
        public string requestId;
    }
}
