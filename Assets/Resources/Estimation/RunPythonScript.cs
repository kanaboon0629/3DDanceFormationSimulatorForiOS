using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class RunPythonScript : MonoBehaviour
{
    public GameObject loadingSpinner; // ローディングスピナーのUIオブジェクト
    public Slider progressBar; // 進行状況バーのUIオブジェクト

    private IEnumerator Start()
    {
        yield return null;

        string url = PlayerPrefs.GetString("url");
        string startStr = PlayerPrefs.GetString("start");
        string endStr = PlayerPrefs.GetString("end");

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(startStr) || string.IsNullOrEmpty(endStr))
        {
            Debug.LogError("URL or start/end parameters are missing.");
            yield break;
        }

        if (!int.TryParse(startStr, out int start) || !int.TryParse(endStr, out int end))
        {
            Debug.LogError("Start or end parameters are not valid integers.");
            yield break;
        }

        loadingSpinner.SetActive(true);
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0f;

        yield return StartCoroutine(SendRequest(url, start, end));

        loadingSpinner.SetActive(false);
        progressBar.gameObject.SetActive(false);
    }

    private IEnumerator SendRequest(string url, int start, int end)
    {
        var requestData = new RequestData
        {
            url = url,
            start = start,
            end = end
        };
        var jsonData = JsonUtility.ToJson(requestData);

        Debug.Log($"Sending JSON data: {jsonData}");

        using (UnityWebRequest request = new UnityWebRequest("http://192.168.1.4:5000/run-script", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Request Error: {request.error}");
            }
            else
            {
                // Save the downloaded JSON file
                string jsonFilePath = Path.Combine(Application.persistentDataPath, "output.json");
                File.WriteAllBytes(jsonFilePath, request.downloadHandler.data);
                Debug.Log($"JSON file saved to: {jsonFilePath}");

                // Process the original JSON file
                ProcessJsonFile(jsonFilePath);

                // Create the symmetry file
                CreateSymmetryFile(jsonFilePath);
            }
        }
    }

    private void ProcessJsonFile(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        Debug.Log($"JSON Content: {jsonContent}");
    }

    private void CreateSymmetryFile(string inputFilePath)
    {
        // Define the output file path
        string outputFilePath = inputFilePath.Replace(".json", "Symmetry.json");

        // Process the JSON file to create the symmetry file
        SymmetryJsonProcessor.ProcessJson(inputFilePath, outputFilePath);

        Debug.Log($"Symmetry JSON file created at: {outputFilePath}");
    }

    [System.Serializable]
    public class RequestData
    {
        public string url;
        public int start;
        public int end;
    }
}