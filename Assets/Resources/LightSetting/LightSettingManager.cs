using UnityEngine;
using UnityEngine.UI; 

public class LightSettingManager : MonoBehaviour
{
    public GameObject frontPanel;
    public GameObject horizonPanel;
    public GameObject dyedPanel;
    public GameObject searchPanel;
    public GameObject backfootPanel;
    public GameObject suspensionPanel;

    private string[] NameOfLight = {
        "FrontLight",
        "HorizonLight",
        "DyedLight",
        "SearchLight",
        "BackfootLight",
        "SuspensionLight"
    };

    // オブジェクトデータを保存するメソッド
    public void SaveLightData()
    {
        GameObject[] allPanel = {
            frontPanel,
            horizonPanel,
            dyedPanel,
            searchPanel,
            backfootPanel,
            suspensionPanel
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
            else if (panel == backfootPanel)
            {
                Dropdown backfootDropdown = panel.GetComponentInChildren<Dropdown>();
                if (backfootDropdown != null)
                {
                    PlayerPrefs.SetInt(NameOfLight[i] + "_Intensity", backfootDropdown.value);
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
            // SuspensionPanel に個数情報の保存
            else if (panel == suspensionPanel)
            {
                Dropdown suspensionDropdown = panel.GetComponentInChildren<Dropdown>();
                if (suspensionDropdown != null)
                {
                    PlayerPrefs.SetInt(NameOfLight[i] + "_Count", suspensionDropdown.value);
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
            backfootPanel,
            suspensionPanel
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
            // BackfootPanel に強弱情報をロード
            else if (panel == backfootPanel)
            {
                Dropdown backfootDropdown = panel.GetComponentInChildren<Dropdown>();
                if (backfootDropdown != null)
                {
                    backfootDropdown.value = PlayerPrefs.GetInt(NameOfLight[i] + "_Intensity", 0);
                }
                else
                {
                    Debug.LogWarning($"Dropdown not found in {panel.name}");
                }
            }
            // SuspensionPanel に個数情報をロード
            else if (panel == suspensionPanel)
            {
                Dropdown suspensionDropdown = panel.GetComponentInChildren<Dropdown>();
                if (suspensionDropdown != null)
                {
                    suspensionDropdown.value = PlayerPrefs.GetInt(NameOfLight[i] + "_Count", 0);
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
