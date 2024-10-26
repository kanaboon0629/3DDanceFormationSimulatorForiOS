using UnityEngine;
using UnityEngine.UI;

public class NumberPicker : MonoBehaviour
{
    public Dropdown numberDropdown;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Button confirmButton;

    void Start()
    {
        // ドロップダウンの初期値を設定
        numberDropdown.value = 0;

        // ボタンがクリックされたときのイベント
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        // プレイヤーの設定から初期値をロード
        int savedValue = PlayerPrefs.GetInt("ObjectCount", 1); // デフォルトは1
        numberDropdown.value = savedValue - 1; // プレイヤーの設定から取得するためには -1
        // スライダーの色データをロード
        redSlider.value = PlayerPrefs.GetFloat("Costumes_Red", 1f);
        greenSlider.value = PlayerPrefs.GetFloat("Costumes_Green", 1f);
        blueSlider.value = PlayerPrefs.GetFloat("Costumes_Blue", 1f);
    }
    void OnConfirmButtonClicked()
    {
        int numberOfPeople = numberDropdown.value + 1; // +1 はオプションの開始値が1から
        Debug.Log("Number of Members: " + numberOfPeople);
        
        // プレイヤーの設定に人数を保存
        PlayerPrefs.SetInt("ObjectCount", numberOfPeople);

        // スライダーの色データを保存
        PlayerPrefs.SetFloat("Costumes_Red", redSlider.value);
        PlayerPrefs.SetFloat("Costumes_Green", greenSlider.value);
        PlayerPrefs.SetFloat("Costumes_Blue", blueSlider.value);
        
        PlayerPrefs.Save(); // 保存
    }
}
