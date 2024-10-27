using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoGalleryManager : MonoBehaviour
{
    public Text resultText;  // 結果を表示するText
    public Color successColor = Color.blue; // 成功時の文字色（青）
    public Color errorColor = Color.red;    // エラー時の文字色（赤）
    public VideoPlayer videoPlayer;  // 動画の長さを取得するために使用するVideoPlayer

    private void Start()
    {
        // VideoPlayerのターゲットテクスチャは今回は使用しません
        // 動画の長さを確認するために使うだけです
        if (videoPlayer == null)
        {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
        }
        
        // 動画の自動再生を無効にする
        videoPlayer.playOnAwake = false;
    }

    public void OpenGalleryForVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            if (path != null)
            {
                // 動画の長さをチェックする処理を開始
                CheckVideoLength(path);
            }
            else
            {
                resultText.text = "Video selection canceled";
                resultText.color = errorColor; // エラーメッセージを赤色に
            }
        }, "Select a video", "video/*");
    }

    private void CheckVideoLength(string path)
    {
        // VideoPlayerを使って動画の長さをチェック
        videoPlayer.url = path;
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (source) =>
        {
            double videoLength = videoPlayer.length;  // 動画の長さ（秒）

            //20秒以上の動画は使用できないようにする
            if (videoLength > 20.0)
            {
                resultText.text = "The video used must be less than 20 seconds long";
                resultText.color = errorColor; // エラーメッセージを赤色に
                PlayerPrefs.DeleteKey("selectedVideoPath");  // 動画パスを保存しない
            }
            else
            {
                PlayerPrefs.SetString("selectedVideoPath", path);
                resultText.text = "Video selected correctly";
                resultText.color = successColor; // 成功メッセージを青色に
            }
        };
    }

    public bool IsVideoSelected()
    {
        string selectedVideoPath = PlayerPrefs.GetString("selectedVideoPath", null);
        return !string.IsNullOrEmpty(selectedVideoPath);
    }
}