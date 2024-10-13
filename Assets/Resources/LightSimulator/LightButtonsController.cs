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
    private float minSuspensionLightAngle = 5f; // スライダーの最小値
    private float maxSuspensionLightAngle = 70f; // スライダーの最大値
    private float minPinspotLightAngle = 5f; // スライダーの最小値
    private float maxPinspotLightAngle = 20f; // スライダーの最大値
    private LightControlWithJoystick lightControlWithJoystick; // LightButtonsControllerのインスタンス

    private void Start()
    {
        // LightControlWithJoystickのインスタンスを取得
        lightControlWithJoystick = FindObjectOfType<LightControlWithJoystick>();
        if (lightControlWithJoystick == null)
        {
            Debug.LogError("lightControlWithJoystickが見つかりません。");
            return;
        }

        int neededSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Toggle");
        int neededPinspotLight = PlayerPrefs.GetInt("PinspotLight_Toggle");

        // サス調整とピンスポ調整が両方必要な場合のみ切り替えボタンを表示
        changeButton.SetActive(neededSuspensionLight == 1 && neededPinspotLight == 1);

        // サス調整がある場合
        if (neededSuspensionLight == 1)
        {
            susPanel.SetActive(true); // サスパネルを表示
            pinPanel.SetActive(false);
            // スライダーの最小値と最大値を設定
            slider.minValue = minSuspensionLightAngle;
            slider.maxValue = maxSuspensionLightAngle;
        }
        else
        {
            susPanel.SetActive(false);
            pinPanel.SetActive(true); // サス調整がない場合ピンパネルを表示
            // スライダーの最小値と最大値を設定
            slider.minValue = minPinspotLightAngle;
            slider.maxValue = maxPinspotLightAngle;
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
        bool isSusPanelActive = susPanel.activeSelf;
        
        // パネルの表示を入れ替える
        susPanel.SetActive(!isSusPanelActive);
        pinPanel.SetActive(isSusPanelActive);

        float angele = lightControlWithJoystick.NowLightAngle(); // サスライトの角度に更新
        // スライダーの最小値と最大値を設定
        if (susPanel.activeSelf)
        {
            slider.minValue = minSuspensionLightAngle;
            slider.maxValue = maxSuspensionLightAngle;
        }
        else
        {
            slider.minValue = minPinspotLightAngle;
            slider.maxValue = maxPinspotLightAngle;
        }
        // 切り替えた後にスライダーの値を更新
        slider.value = angele; // サスライトの角度に更新
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
            // スライダーの最小値と最大値を設定
            slider.minValue = minSuspensionLightAngle;
            slider.maxValue = maxSuspensionLightAngle;
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
            // スライダーの最小値と最大値を設定
            slider.minValue = minPinspotLightAngle;
            slider.maxValue = maxPinspotLightAngle;
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