using UnityEngine;
using UnityEngine.UI;

public class ToggleUIController : MonoBehaviour
{
    // チェックボックスのToggleコンポーネントをインスペクタで指定
    public Toggle toggle;

    void Start()
    {
        // Toggleの初期値に基づいて自身の表示状態を設定
        gameObject.SetActive(toggle.isOn);

        // Toggleの状態が変更されたときに呼び出されるリスナーを追加
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Toggleの値が変更されたときに呼び出されるメソッド
    private void OnToggleValueChanged(bool isOn)
    {
        // 自身の表示・非表示を切り替え
        gameObject.SetActive(isOn);
    }
}
