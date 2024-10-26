using System;
using UnityEngine;
using UnityEngine.UI;

public class RealtimeYotubeInputValidation : MonoBehaviour
{
    public InputField urlField;
    public InputField startField;
    public InputField endField;
    public Text message;
    public Color successColor = Color.blue; // 成功時の文字色（青）
    public Color errorColor = Color.red;    // エラー時の文字色（赤）

    void Start()
    {
        // 保存されているURL、Start、Endの情報を復元
        if (PlayerPrefs.HasKey("url"))
        {
            urlField.text = PlayerPrefs.GetString("url");
            Debug.Log("Loaded saved URL: " + urlField.text);
        }
        if (PlayerPrefs.HasKey("start"))
        {
            startField.text = PlayerPrefs.GetString("start");
            Debug.Log("Loaded saved Start: " + startField.text);
        }
        if (PlayerPrefs.HasKey("end"))
        {
            endField.text = PlayerPrefs.GetString("end");
            Debug.Log("Loaded saved End: " + endField.text);
        }

        // InputFieldにイベントリスナーを追加
        urlField.onValueChanged.AddListener(delegate { ValidateAllFields(); });
        startField.onValueChanged.AddListener(delegate { ValidateAllFields(); });
        endField.onValueChanged.AddListener(delegate { ValidateAllFields(); });

        message.text = ""; // 初期状態では空にする
    }

    private void ValidateAllFields()
    {
        // URLフィールドのバリデーション
        if (!ValidateUrl())
        {
            return;
        }

        // StartとEndフィールドのバリデーション
        if (!ValidateStartAndEnd())
        {
            return;
        }

        // すべてのバリデーションが通過したら成功メッセージを表示
        message.text = "Valid URL";
        message.color = successColor; // 成功メッセージを青色に
    }

    private bool ValidateUrl()
    {
        if (!IsValidUrl(urlField.text))
        {
            message.text = "Invalid URL";
            message.color = errorColor; // エラーメッセージを赤色に
            return false;
        }

        message.text = "";
        return true;
    }

    private bool ValidateStartAndEnd()
    {
        if (!IsNaturalNumber(startField.text) || !IsNaturalNumber(endField.text))
        {
            message.text = "Start and End must be natural numbers.";
            message.color = errorColor; // エラーメッセージを赤色に
            return false;
        }

        int start = int.Parse(startField.text);
        int end = int.Parse(endField.text);

        if (end <= start)
        {
            message.text = "End must be greater than Start.";
            message.color = errorColor; // エラーメッセージを赤色に
            return false;
        }

        if (end - start > 20)
        {
            message.text = "The difference between End and Start must be 20 seconds or less.";
            message.color = errorColor; // エラーメッセージを赤色に
            return false;
        }

        message.text = "";
        return true;
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool IsNaturalNumber(string value)
    {
        return int.TryParse(value, out int number) && number >= 0;  // 0も受け入れる
    }
}
