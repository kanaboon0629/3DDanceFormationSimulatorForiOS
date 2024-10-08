using UnityEngine;
using UnityEngine.UI;

public class AllLightSaverAndLoader : MonoBehaviour
{
    public GameObject suspensionLightPrefab; // 複製するアセットのプレハブ
    public int numberOfSuspensionLight; // 個数
    public GameObject suspensionLightParent; // SuspensionLight の親オブジェクト
    public GameObject frontLightParent; // FrontLight の親オブジェクト
    public GameObject horizonLightParent; // HorizonLight の親オブジェクト
    public GameObject dyedLightParent; // DyedLight の親オブジェクト
    public GameObject searchLightParent; // SearchLight の親オブジェクト
    public GameObject backfootLightParent; // BackFootLight の親オブジェクト

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
        if (PlayerPrefs.GetInt("SuspensionLight_Toggle") == 1){
            numberOfSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Count", 1); // デフォルトは1
            // SuspensionLightを配置
            ArrangeAssets(numberOfSuspensionLight);

            // SuspensionLightの位置データをロード
            if (PlayerPrefs.HasKey("SuspensionLightPosX_0"))
            {
                LoadSuspensionLightData();
            }
        }

        // ライトの有無によって表示        
        frontLightParent.SetActive(PlayerPrefs.GetInt("FrontLight_Toggle") == 1);
        horizonLightParent.SetActive(PlayerPrefs.GetInt("HorizonLight_Toggle") == 1);
        dyedLightParent.SetActive(PlayerPrefs.GetInt("DyedLight_Toggle") == 1);
        searchLightParent.SetActive(PlayerPrefs.GetInt("SearchLight_Toggle") == 1);
        backfootLightParent.SetActive(PlayerPrefs.GetInt("BackfootLight_Toggle") == 1);

        // 各ライトの色を適用
        ArrangeColor(suspensionLightParent, "SuspensionLight");
        ArrangeColor(frontLightParent, "FrontLight");
        ArrangeColor(horizonLightParent, "HorizonLight");
        ArrangeColor(dyedLightParent, "DyedLight");
        ArrangeColor(searchLightParent, "SearchLight");
        ArrangeColor(backfootLightParent, "BackfootLight");

        if (searchLightParent.activeSelf){
            ArrangeAngle(searchLightParent, "SearchLight");
        }
        if (backfootLightParent.activeSelf){
            ArrangeIntensity(backfootLightParent, "BackfootLight");
        }
    }
    // 各ライトの色をロードして適用する
    void ArrangeColor(GameObject lightParent, string lightName)
    {
        if (!lightParent.activeSelf) return;

        // プレイヤープリファレンスからRGB値をロード
        float red = PlayerPrefs.GetFloat(lightName + "_Red", 1.0f);
        float green = PlayerPrefs.GetFloat(lightName + "_Green", 1.0f);
        float blue = PlayerPrefs.GetFloat(lightName + "_Blue", 1.0f);

        // lightParent内の全てのLightコンポーネントを持つ子オブジェクトの色を変更
        Light[] lights = lightParent.GetComponentsInChildren<Light>();  // 子オブジェクトを含むすべてのLightを取得
        if (lights.Length > 0)
        {
            foreach (Light light in lights)
            {
                light.color = new Color(red, green, blue);
            }
        }
        else
        {
            Debug.LogWarning($"No Light found on {lightParent.name} or its children.");
        }
    }

    void ArrangeAngle(GameObject lightParent, string lightName){
        int angle = PlayerPrefs.GetInt(lightName + "_Angle");
        Light[] lights = lightParent.GetComponentsInChildren<Light>();  // 子オブジェクトを含むすべてのLightを取得

        if (lights.Length > 0){
            //クロスの場合
            if (angle == 1){
                for (int i = 0; i < lights.Length; i++)
                {
                    Light light = lights[i];
                    // 偶数インデックスはrotationXを-60度、奇数インデックスは-120度に設定
                    if (i % 2 == 0)
                    {
                        light.transform.rotation = Quaternion.Euler(-60f, light.transform.rotation.eulerAngles.y, light.transform.rotation.eulerAngles.z);
                    }
                    else
                    {
                        light.transform.rotation = Quaternion.Euler(-120f, light.transform.rotation.eulerAngles.y, light.transform.rotation.eulerAngles.z);
                    }
                }
            //ストレートの場合
            }else{
                    for (int i = 0; i < lights.Length; i++)
                    {
                        Light light = lights[i];
                        light.transform.rotation = Quaternion.Euler(-90f, light.transform.rotation.eulerAngles.y, light.transform.rotation.eulerAngles.z);
                    }
            }
        }else
        {
            Debug.LogWarning($"No Light found on {lightParent.name} or its children.");
        }
    }
    void ArrangeIntensity(GameObject lightParent, string lightName){
        int intensity = PlayerPrefs.GetInt(lightName + "_Intensity");
        Light[] lights = lightParent.GetComponentsInChildren<Light>();  // 子オブジェクトを含むすべてのLightを取得
        if (lights.Length > 0){
            if (intensity == 1){
                for (int i = 0; i < lights.Length; i++)
                {   
                    lights[i].intensity = 2;
                }
            }else{
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].intensity = 1;
                }
            }
        }else
        {
            Debug.LogWarning($"No Light found on {lightParent.name} or its children.");
        }
    }

    void ArrangeAssets(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 複製して位置を設定し、親を suspensionLightParent に設定
            GameObject newAsset = Instantiate(suspensionLightPrefab, suspensionLightParent.transform);

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
