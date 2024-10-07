using UnityEngine;
using UnityEngine.UI;

public class SuspensionLightSaverAndLoader : MonoBehaviour
{
    public GameObject assetPrefab; // 複製するアセットのプレハブ
    public int numberOfSuspensionLight; // 個数
    public GameObject suspensionLightParent; // SuspensionLight の親オブジェクト

    private Vector3[] positions = {
        new Vector3(2.98f, -1.39796f, -1.45f),
        new Vector3(2.98f, -1.39796f, 1.45f),
        new Vector3(2.98f, -1.39796f, -4.3f),
        new Vector3(2.98f, -1.39796f, 4.3f),
        new Vector3(2.98f, -1.39796f, -7.15f),
        new Vector3(2.98f, -1.39796f, 7.15f)
    };

    void Start()
    {
        numberOfSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Count", 1); // デフォルトは1
        ArrangeAssets(numberOfSuspensionLight);
        
        // 最初にこの画面を遷移した場合のみ初期位置を設定
        if (PlayerPrefs.HasKey("SuspensionLightPosX_0"))
        {
            LoadSuspensionLightData();
        }
    }

    void ArrangeAssets(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 複製して位置を設定し、親を suspensionLightParent に設定
            GameObject newAsset = Instantiate(assetPrefab, suspensionLightParent.transform);

            if (i < positions.Length)
            {
                newAsset.transform.localPosition = positions[i]; // 親のローカル座標に基づいて配置
            }
            else
            {
                Debug.LogWarning("アセットの数がポジションの数を超えています。");
            }
        }
    }

    public void SaveSuspensionLightData()
    {
        Light[] lights = suspensionLightParent.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            Light light = lights[i]; // Lightコンポーネントを取得

            PlayerPrefs.SetFloat("SuspensionLightPosX_" + i, light.transform.position.x);
            PlayerPrefs.SetFloat("SuspensionLightPosY_" + i, light.transform.position.y);
            PlayerPrefs.SetFloat("SuspensionLightPosZ_" + i, light.transform.position.z);
            PlayerPrefs.SetFloat("SuspensionLightSpotAngle_" + i, light.spotAngle); // 修正点
        }
        PlayerPrefs.Save();
    }

    public void LoadSuspensionLightData()
    {
        Light[] lights = suspensionLightParent.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            Light light = lights[i]; // Lightコンポーネントを取得
            if (PlayerPrefs.HasKey("SuspensionLightPosX_" + i))
            {
                float x = PlayerPrefs.GetFloat("SuspensionLightPosX_" + i);
                float y = PlayerPrefs.GetFloat("SuspensionLightPosY_" + i);
                float z = PlayerPrefs.GetFloat("SuspensionLightPosZ_" + i);
                
                light.transform.position = new Vector3(x, y, z); // ポジションを設定
                light.spotAngle = PlayerPrefs.GetFloat("SuspensionLightSpotAngle_" + i); // 修正点
            }
        }
    }
}
