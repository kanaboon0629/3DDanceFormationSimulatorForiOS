using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;

public class InputFieldHandler : MonoBehaviour
{
    public InputField ipInputField; // IPアドレスを入力するフィールド
    public InputField urlInputField;
    public InputField startInputField;
    public InputField endInputField;
    public Text errorMessageFromVideofile;
    public Text errorMessageFromYoutube;
    public Text errorMessageFromIPAddress; // IPアドレスのエラーメッセージ用
    public VideoGalleryManager videoGalleryManager; // VideoGalleryManagerの参照
    public TabController tabController; // TabControllerの参照

    public void OnSubmit(string nextSceneName)
    {
        if (tabController == null || videoGalleryManager == null)
        {
            return;
        }

        int currentTabIndex = tabController.GetCurrentTabIndex(); // 現在のタブインデックスを取得

        if (currentTabIndex == 0)
        {
            // 0個目のタブ: ビデオファイルの選択チェック
            if (!IsVideoSelected())
            {
                DisplayErrorMessage("Please select a video.", errorMessageFromVideofile);
                return;
            }
        }
        else if (currentTabIndex == 1)
        {
            // 1個目のタブ: URLと秒数のチェック
            string url = urlInputField.text;
            string start = startInputField.text;
            string end = endInputField.text;

            string validationError = ValidateInputs(url, start, end);

            if (!string.IsNullOrEmpty(validationError))
            {
                DisplayErrorMessage(validationError, errorMessageFromYoutube);
                return;
            }

            PlayerPrefs.SetString("url", url);
            PlayerPrefs.SetString("start", start);
            PlayerPrefs.SetString("end", end);
        }

        // IPアドレスフィールドのエラーハンドリング
        string ipAddress = ipInputField.text;
        string ipValidationError = ValidateIPAddress(ipAddress);
        if (!string.IsNullOrEmpty(ipValidationError))
        {
            DisplayErrorMessage(ipValidationError, errorMessageFromIPAddress);
            return;
        }

        PlayerPrefs.SetString("IPAddress", ipAddress); // 正しいIPアドレスを保存
        PlayerPrefs.SetInt("tabCount", currentTabIndex);

        // 入力が正しい場合、次のシーンに移動
        SceneManager.LoadScene(nextSceneName);
    }

    private bool IsVideoSelected()
    {
        return videoGalleryManager != null && videoGalleryManager.IsVideoSelected();
    }

    private void DisplayErrorMessage(string message, Text errorMessage)
    {
        if (errorMessage != null)
        {
            errorMessage.text = message;
        }
    }

    private string ValidateInputs(string url, string start, string end)
    {
        if (!IsValidUrl(url))
        {
            return "Invalid URL.";
        }

        if (!IsNaturalNumber(start, out int startValue))
        {
            return "Start must be a natural number.";
        }

        if (!IsNaturalNumber(end, out int endValue))
        {
            return "End must be a natural number.";
        }

        if (endValue <= startValue)
        {
            return "End must be greater than Start.";
        }

        if (endValue - startValue > 30)
        {
            return "The difference between End and Start must be 30 seconds or less.";
        }

        return string.Empty; // No errors
    }

    private string ValidateIPAddress(string ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            return "IP Address cannot be empty.";
        }

        // IPアドレスが正しい形式かどうかを確認
        if (!IPAddress.TryParse(ipAddress, out _))
        {
            return "Invalid IP Address format.";
        }

        return string.Empty; // 正しい場合はエラーメッセージなし
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool IsNaturalNumber(string value, out int number)
    {
        // 0も自然数として許可するため、number >= 0
        return int.TryParse(value, out number) && number >= 0;
    }
}
