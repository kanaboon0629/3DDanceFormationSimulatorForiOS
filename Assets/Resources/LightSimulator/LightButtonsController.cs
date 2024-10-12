using UnityEngine;
using UnityEngine.UI;

public class LightButtonsController : MonoBehaviour
{
    public GameObject changeButton; // 切り替えボタン
    public Slider slider; // スライダーをSlider型に変更
    public GameObject susPanel;
    public GameObject pinPanel; 
    public GameObject[] susButtons; // サス用のタブボタン
    public GameObject[] pinButtons; // ピン用のタブボタン
    public Color selectedColor = Color.white; // 選択中の色
    public Color unselectedColor = new Color(1f, 1f, 1f, 0.5f); // 非選択の色（白の半透明）

    public int initialTabIndex = 0; // 初期表示するタブのインデックス

    private int currentSusTabIndex; // 現在のサスタブのインデックス
    private int currentPinTabIndex; // 現在のピンタブのインデックス
    private float minLightAngle = 5f; // スライダーの最小値
    private float maxLightAngle = 70f; // スライダーの最大値

    private void Start()
    {
        int neededSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Toggle");
        int neededPinspotLight = PlayerPrefs.GetInt("PinspotLight_Toggle");

        // サス調整とピンスポ調整が両方必要な場合のみ切り替えボタンを表示
        changeButton.SetActive(neededSuspensionLight == 1 && neededPinspotLight == 1);
        // スライダーの最小値と最大値を設定
        slider.minValue = minLightAngle;
        slider.maxValue = maxLightAngle;

        // サス調整がある場合
        if (neededSuspensionLight == 1)
        {
            susPanel.SetActive(true); // サスパネルを表示
            pinPanel.SetActive(false);
        }
        else
        {
            susPanel.SetActive(false);
            pinPanel.SetActive(true); // サス調整がない場合ピンパネルを表示
        }

        int numberOfSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Count", 0);
        int numberOfPinspotLight = PlayerPrefs.GetInt("PinspotLight_Count", 0);
        SetTabVisibility(susButtons, numberOfSuspensionLight);
        SetTabVisibility(pinButtons, numberOfPinspotLight);

        // 初期タブの設定
        InitializeTab(susButtons, ref currentSusTabIndex);
        InitializeTab(pinButtons, ref currentPinTabIndex);
    }

    private void SetTabVisibility(GameObject[] buttons, int numberOfLights)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(i < numberOfLights);
        }
    }
    public void ChangePanel()
    {
        // susPanelの表示状態をpinPanelに入れ替える
        bool isSusPanelActive = susPanel.activeSelf;
        // パネルの表示を入れ替える
        susPanel.SetActive(!isSusPanelActive);  // susPanelの表示を反転
        pinPanel.SetActive(isSusPanelActive);   // pinPanelの表示を反転
    }
    
    public void SwitchLightSource(int index)
    {
        if (susPanel.activeSelf)
        {
            currentSusTabIndex = index;
            foreach (var tab in susButtons)
            {
                Button tabButton = tab.GetComponent<Button>();
                ColorBlock colors = tabButton.colors;
                colors.normalColor = unselectedColor;
                tabButton.colors = colors;
            }

            Button selectedTabButton = susButtons[index].GetComponent<Button>();
            ColorBlock selectedColors = selectedTabButton.colors;
            selectedColors.normalColor = selectedColor;
            selectedTabButton.colors = selectedColors;
        }
        else
        {
            currentPinTabIndex = index;
            foreach (var tab in pinButtons)
            {
                Button tabButton = tab.GetComponent<Button>();
                ColorBlock colors = tabButton.colors;
                colors.normalColor = unselectedColor;
                tabButton.colors = colors;
            }

            Button selectedTabButton = pinButtons[index].GetComponent<Button>();
            ColorBlock selectedColors = selectedTabButton.colors;
            selectedColors.normalColor = selectedColor;
            selectedTabButton.colors = selectedColors;
        }
    }

    private void InitializeTab(GameObject[] buttons, ref int currentTabIndex)
    {
        currentTabIndex = 0; // 初期タブのインデックスを0に設定

        // すべてのボタンの色を非選択状態に設定
        foreach (var tab in buttons)
        {
            Button tabButton = tab.GetComponent<Button>();
            ColorBlock colors = tabButton.colors;
            colors.normalColor = unselectedColor;
            tabButton.colors = colors;
        }

        // 最初のタブを選択状態に設定
        Button selectedTabButton = buttons[0].GetComponent<Button>();
        ColorBlock selectedColors = selectedTabButton.colors;
        selectedColors.normalColor = selectedColor;
        selectedTabButton.colors = selectedColors;
    }
}