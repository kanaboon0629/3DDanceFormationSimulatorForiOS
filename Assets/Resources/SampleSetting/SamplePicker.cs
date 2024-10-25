using UnityEngine;
using UnityEngine.UI;

public class SamplePicker : MonoBehaviour
{
    public Dropdown numberDropdown;
    public Button confirmButton;

    void Start()
    {
        // ドロップダウンの初期値を設定
        numberDropdown.value = 0;

        // ボタンがクリックされたときのイベント
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        // 設定から初期値をロード
        int savedValue = PlayerPrefs.GetInt("SelectedSample", 0);
        numberDropdown.value = savedValue;
    }

    void OnConfirmButtonClicked()
    {
        int selectedSample = numberDropdown.value;
        Debug.Log("Number of Sample: " + selectedSample);
        
        PlayerPrefs.SetInt("SelectedSample", selectedSample);
        PlayerPrefs.Save(); // 保存
    }
}