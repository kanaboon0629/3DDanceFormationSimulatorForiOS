using UnityEngine;
using UnityEngine.UI;

public class SuspensionLightController : MonoBehaviour
{
    public GameObject[] tabs; // タブボタンの配列
    public Color selectedColor = Color.white; // 選択中の色
    public Color unselectedColor = new Color(1f, 1f, 1f, 0.5f); // 非選択の色（白の半透明）

    public int initialTabIndex = 0; // 初期表示するタブのインデックス

    private int currentTabIndex; // 現在のタブのインデックス

    private void Start()
    {
        int numberOfSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Count", 1);

        // ボタンの表示を設定
        SetTabVisibility(numberOfSuspensionLight);

        // 初期タブの設定
        SwitchLightSource(initialTabIndex);
    }

    private void SetTabVisibility(int numberOfSuspensionLight)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            // iが表示するタブの数より小さい場合は表示、そうでなければ非表示
            tabs[i].SetActive(i < numberOfSuspensionLight);
        }
    }

    public void SwitchLightSource(int index)
    {
        currentTabIndex = index; // 現在のタブのインデックスを更新

        foreach (var tab in tabs)
        {
            Button tabButton = tab.GetComponent<Button>();
            ColorBlock colors = tabButton.colors;
            colors.normalColor = unselectedColor;
            tabButton.colors = colors;
        }

        // 選択されたタブの色を変更
        Button selectedTabButton = tabs[index].GetComponent<Button>();
        ColorBlock selectedColors = selectedTabButton.colors;
        selectedColors.normalColor = selectedColor;
        selectedTabButton.colors = selectedColors;
    }

    public int GetCurrentTabIndex()
    {
        return currentTabIndex;
    }
}
