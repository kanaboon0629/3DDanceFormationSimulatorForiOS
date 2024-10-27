using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class RealtimeIPAddressInputValidation : MonoBehaviour
{
    public InputField ipInputField; // IPアドレスを入力するフィールド
    public Text message; // エラーメッセージまたは成功メッセージ表示用のテキスト
    public Color successColor = Color.blue; // 成功時の文字色（青）
    public Color errorColor = Color.red;    // エラー時の文字色（赤）

    void Start()
    {
        // 保存されているIPアドレスがあれば、InputFieldに表示
        if (PlayerPrefs.HasKey("IPAddress"))
        {
            string savedIpAddress = PlayerPrefs.GetString("IPAddress");
            ipInputField.text = savedIpAddress;
            Debug.Log("Loaded saved IP address: " + savedIpAddress);
        }
        
        // IPアドレスフィールドにイベントリスナーを追加
        ipInputField.onValueChanged.AddListener(delegate { ValidateIPAddress(); });

        // 初期状態ではメッセージを非表示に
        message.text = "";
    }

    private void ValidateIPAddress()
    {
        string ipAddress = ipInputField.text;

        // IPアドレスが空の場合はメッセージをクリア
        if (string.IsNullOrEmpty(ipAddress))
        {
            message.text = "";
            return;
        }

        // IPアドレスが有効かどうかを確認
        if (!IPAddress.TryParse(ipAddress, out _))
        {
            message.text = "Invalid IP Address";
            message.color = errorColor; // エラーメッセージを赤色に
        }
        else
        {
            message.text = "Valid IP Address";
            message.color = successColor; // 成功メッセージを青色に
        }
    }
}