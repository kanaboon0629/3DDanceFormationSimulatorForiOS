using UnityEngine;
using UnityEngine.UI; 

public class LightSettingSaverAndLoader : MonoBehaviour
{
    public GameObject frontPanel;
    public GameObject horizonPanel;
    public GameObject dyedPanel;
    public GameObject searchPanel;
    public GameObject floorPanel;
    public GameObject suspensionPanel;
    public GameObject pinspotPanel;


    private string[] NameOfLight = {
        "FrontLight",
        "HorizonLight",
        "DyedLight",
        "SearchLight",
        "FloorLight",
        "SuspensionLight",
        "PinspotLight"
    };

    // オブジェクトデータを保存するメソッド
    public void SaveLightData()
    {
        GameObject[] allPanel = {
            frontPanel,
            horizonPanel,
            dyedPanel,
            searchPanel,
            floorPanel,
            suspensionPanel,
            pinspotPanel
        };

        // 有無と色について保存
        for (int i = 0; i < allPanel.Length; i++)
        {
            GameObject panel = allPanel[i];

            // Toggleの状態を保存
            Toggle toggle = panel.GetComponentInChildren<Toggle>();
            if (toggle != null)
            {
                PlayerPrefs.SetInt(NameOfLight[i] + "_Toggle", toggle.isOn ? 1 : 0);
            }
            else
            {
                Debug.LogWarning($"Toggle not found in {panel.name}");
            }

            // スライダーの色データを保存
            Slider redSlider = panel.transform.Find("SettingPanel/ColorPickerPanel/RedSlider")?.GetComponent<Slider>();
            Slider greenSlider = panel.transform.Find("SettingPanel/ColorPickerPanel/GreenSlider")?.GetComponent<Slider>();
            Slider blueSlider = panel.transform.Find("SettingPanel/ColorPickerPanel/BlueSlider")?.GetComponent<Slider>();

            if (redSlider != null && greenSlider != null && blueSlider != null)
            {
                PlayerPrefs.SetFloat(NameOfLight[i] + "_Red", redSlider.value);
                PlayerPrefs.SetFloat(NameOfLight[i] + "_Green", greenSlider.value);
                PlayerPrefs.SetFloat(NameOfLight[i] + "_Blue", blueSlider.value);
            }
            else
            {
                Debug.LogWarning($"Sliders not found in {panel.name}");
            }

            // SearchPanel に角度情報の保存
            if (panel == searchPanel)
            {
                Dropdown searchDropdown = panel.GetComponentInChildren<Dropdown>();
                if (searchDropdown != null)
                {
                    PlayerPrefs.SetInt(NameOfLight[i] + "_Angle", searchDropdown.value);
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
            // BackfootPanel に強弱情報の保存
            else if (panel == floorPanel)
            {
                // "SettingPanel/isBackfootToggle" のトグルを取得
                Toggle isBackfoot = panel.transform.Find("SettingPanel/isBackfootToggle").GetComponent<Toggle>();
                if (isBackfoot != null)
                {
                    PlayerPrefs.SetInt(NameOfLight[i] + "_Backfoot", isBackfoot.isOn ? 1 : 0);
                }
                else
                {
                    Debug.LogWarning($"Toggle not found in {panel.name}");
                }

                // "SettingPanel/BackfootToggles" の子トグルを取得し、保存
                Transform backfootToggles = panel.transform.Find("SettingPanel/BackfootToggles");
                if (backfootToggles != null)
                {
                    for (int j = 0; j < backfootToggles.childCount; j++)
                    {
                        Toggle backfootNumToggle = backfootToggles.GetChild(j).GetComponent<Toggle>();
                        if (backfootNumToggle != null)
                        {
                            PlayerPrefs.SetInt(NameOfLight[i] + "_BackfootNum" + j, backfootNumToggle.isOn ? 1 : 0);
                        }
                        else
                        {
                            Debug.LogWarning($"Toggle {j} not found in BackfootToggles of {panel.name}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"BackfootToggles not found in {panel.name}");
                }
            }
            // SuspensionPanel に個数情報の保存
            else if (panel == suspensionPanel)
            {
                Dropdown suspensionDropdown = panel.GetComponentInChildren<Dropdown>();
                if (suspensionDropdown != null)
                {
                    PlayerPrefs.SetInt(NameOfLight[i] + "_Count", suspensionDropdown.value + 1);
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
            else if (panel == pinspotPanel)
            {
                Dropdown pinspotDropdown = panel.GetComponentInChildren<Dropdown>();
                if (pinspotDropdown != null)
                {
                    PlayerPrefs.SetInt(NameOfLight[i] + "_Count", pinspotDropdown.value + 1);
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
        }

        PlayerPrefs.Save();  // PlayerPrefs にデータを保存
    }

    // 保存データをロードして反映するメソッド
    public void LoadLightData()
    {
        GameObject[] allPanel = {
            frontPanel,
            horizonPanel,
            dyedPanel,
            searchPanel,
            floorPanel,
            suspensionPanel,
            pinspotPanel
        };

        for (int i = 0; i < allPanel.Length; i++)
        {
            GameObject panel = allPanel[i];

            // Toggleの状態をロード
            Toggle toggle = panel.GetComponentInChildren<Toggle>();
            if (toggle != null)
            {
                toggle.isOn = PlayerPrefs.GetInt(NameOfLight[i] + "_Toggle", 0) == 1;
            }
            else
            {
                Debug.LogWarning($"Toggle not found in {panel.name}");
            }

            // スライダーの色データをロード
            Slider redSlider = panel.transform.Find("SettingPanel/ColorPickerPanel/RedSlider")?.GetComponent<Slider>();
            Slider greenSlider = panel.transform.Find("SettingPanel/ColorPickerPanel/GreenSlider")?.GetComponent<Slider>();
            Slider blueSlider = panel.transform.Find("SettingPanel/ColorPickerPanel/BlueSlider")?.GetComponent<Slider>();

            if (redSlider != null && greenSlider != null && blueSlider != null)
            {
                redSlider.value = PlayerPrefs.GetFloat(NameOfLight[i] + "_Red", 1f);
                greenSlider.value = PlayerPrefs.GetFloat(NameOfLight[i] + "_Green", 1f);
                blueSlider.value = PlayerPrefs.GetFloat(NameOfLight[i] + "_Blue", 1f);
            }
            else
            {
                Debug.LogWarning($"Sliders not found in {panel.name}");
            }

            // SearchPanel に角度情報をロード
            if (panel == searchPanel)
            {
                Dropdown searchDropdown = panel.GetComponentInChildren<Dropdown>();
                if (searchDropdown != null)
                {
                    searchDropdown.value = PlayerPrefs.GetInt(NameOfLight[i] + "_Angle", 0);
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
            else if (panel == floorPanel)
            {
                // "SettingPanel/isBackfootToggle" のトグルを取得
                Toggle isBackfoot = panel.transform.Find("SettingPanel/isBackfootToggle").GetComponent<Toggle>();
                if (isBackfoot != null)
                {
                    // PlayerPrefsから状態をロード
                    isBackfoot.isOn = PlayerPrefs.GetInt(NameOfLight[i] + "_Backfoot", 0) == 1;
                }
                else
                {
                    Debug.LogWarning($"Toggle not found in {panel.name}");
                }

                // "SettingPanel/BackfootToggles" の子トグルを取得し、ロード
                Transform backfootToggles = panel.transform.Find("SettingPanel/BackfootToggles");
                if (backfootToggles != null)
                {
                    for (int j = 0; j < backfootToggles.childCount; j++)
                    {
                        Toggle backfootNumToggle = backfootToggles.GetChild(j).GetComponent<Toggle>();
                        if (backfootNumToggle != null)
                        {
                            // PlayerPrefsから状態をロード
                            backfootNumToggle.isOn = PlayerPrefs.GetInt(NameOfLight[i] + "_BackfootNum" + j, 0) == 1;
                        }
                        else
                        {
                            Debug.LogWarning($"Toggle {j} not found in BackfootToggles of {panel.name}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"BackfootToggles not found in {panel.name}");
                }
            }
            // SuspensionPanel に個数情報をロード
            else if (panel == suspensionPanel)
            {
                Dropdown suspensionDropdown = panel.GetComponentInChildren<Dropdown>();
                if (suspensionDropdown != null)
                {
                    suspensionDropdown.value = PlayerPrefs.GetInt(NameOfLight[i] + "_Count", 1) - 1;
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
            // PinspotPanel に個数情報をロード
            else if (panel == pinspotPanel)
            {
                Dropdown pinspotDropdown = panel.GetComponentInChildren<Dropdown>();
                if (pinspotDropdown != null)
                {
                    pinspotDropdown.value = PlayerPrefs.GetInt(NameOfLight[i] + "_Count", 1) - 1;
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
        }
    }

    // シーン読み込み時にデータをロードして反映
    private void Start()
    {
        if (PlayerPrefs.HasKey("FrontLight_Toggle"))
        {
            LoadLightData();
        }
        else
        {
            Debug.Log("No saved data found for FrontLight.");
        }
    }
}
