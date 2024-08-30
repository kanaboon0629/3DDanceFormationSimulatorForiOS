using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 動画再生用のVideoPlayer
    public GameObject loadingPanel; // 非表示にする
    public GameObject playPanel; // 非表示にする
    private string videoFilePath;
    public GameObject checkButton;

    void Start()
    {
        playPanel.SetActive(false);
    }

    public void CallDownloadAndPlayVideo()
    {
        loadingPanel.SetActive(false);
        StartCoroutine(DownloadAndPlayVideo());
    }
    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    private IEnumerator DownloadAndPlayVideo()
    {
        string url = "http://192.168.1.4:5000/download-video"; // サーバの動画ダウンロードエンドポイントURL

        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            // サーバにリクエストを送信
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                UnityEngine.Debug.LogError("Error downloading video: " + uwr.error);
            }
            else
            {
                checkButton.SetActive(false);
                // 成功したら動画を保存
                videoFilePath = Path.Combine(Application.persistentDataPath, "downloadedVideo.mp4");
                File.WriteAllBytes(videoFilePath, uwr.downloadHandler.data);
                UnityEngine.Debug.Log("Video downloaded and saved to: " + videoFilePath);

                // VideoPlayerに動画をセットして再生
                videoPlayer.url = videoFilePath;

                playPanel.SetActive(true);
                
                videoPlayer.Play();
            }
        }
    }
}