using UnityEngine;
using UnityEngine.UI;

public class AllLightSaverAndLoader : MonoBehaviour
{
    public GameObject suspensionLightPrefab; // 複製するアセットのプレハブ
    public int numberOfSuspensionLight; // 個数
    public GameObject pinspotLightPrefab; // 複製するアセットのプレハブ
    public int numberOfPinspotLight; // 個数
    
    public GameObject suspensionLightParent; // SuspensionLight の親オブジェクト
    public GameObject frontLightParent; // FrontLight の親オブジェクト
    public GameObject horizonLightParent; // HorizonLight の親オブジェクト
    public GameObject dyedLightParent; // DyedLight の親オブジェクト
    public GameObject searchLightParent; // SearchLight の親オブジェクト
    public GameObject backfootLightParent; // BackFootLight の親オブジェクト
    public GameObject pinspotLightParent; // pinspotLight の親オブジェクト

    private Vector3[] suspensionPositions = {
        new Vector3(2.98f, -1.39796f, -1.45f),
        new Vector3(2.98f, -1.39796f, 1.45f),
        new Vector3(2.98f, -1.39796f, -4.3f),
        new Vector3(2.98f, -1.39796f, 4.3f),
        new Vector3(2.98f, -1.39796f, -7.15f),
        new Vector3(2.98f, -1.39796f, 7.15f)
    };
        private Vector3[] pinspotPositions = {
        new Vector3(24.57f, 15.1f, -1.45f),
        new Vector3(24.57f, 15.1f, 1.45f),
        new Vector3(24.57f, 15.1f, -4.3f),
        new Vector3(24.57f, 15.1f, 4.3f),
        new Vector3(24.57f, 15.1f, -7.15f),
        new Vector3(24.57f, 15.1f, 7.15f)
    };
    private float[] radialSearchLightRotations = {
        -140f,
        -130f,
        -120f,
        -60f,
        -50f,
        -40f
    };

    void Start()
    {
        // SuspensionLightを配置
        if (PlayerPrefs.GetInt("SuspensionLight_Toggle") == 1){
            numberOfSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Count", 1); // デフォルトは1
            ArrangeAssets(numberOfSuspensionLight, suspensionLightPrefab, suspensionLightParent, suspensionPositions);  
        }

        // PinspotLightを配置
        if (PlayerPrefs.GetInt("PinspotLight_Toggle") == 1){
            numberOfPinspotLight = PlayerPrefs.GetInt("PinspotLight_Count", 1); // デフォルトは1
            ArrangeAssets(numberOfPinspotLight, pinspotLightPrefab, pinspotLightParent, pinspotPositions);
        }

        // 2回目以降は位置データをロード
        if (PlayerPrefs.HasKey("SuspensionLightPosX_0") || PlayerPrefs.HasKey("PinspotLightRotX_0"))
        {
            LoadLightPositionAndAngle("SuspensionLight", suspensionLightParent);
            LoadLightRotationAndAngle("PinspotLight", pinspotLightParent);
        }

        // ライトの有無によって表示        
        frontLightParent.SetActive(PlayerPrefs.GetInt("FrontLight_Toggle") == 1);
        horizonLightParent.SetActive(PlayerPrefs.GetInt("HorizonLight_Toggle") == 1);
        dyedLightParent.SetActive(PlayerPrefs.GetInt("DyedLight_Toggle") == 1);
        searchLightParent.SetActive(PlayerPrefs.GetInt("SearchLight_Toggle") == 1);
        backfootLightParent.SetActive(PlayerPrefs.GetInt("BackfootLight_Toggle") == 1);

        // 各ライトの色を適用
        ArrangeColor(suspensionLightParent, "SuspensionLight");
        ArrangeColor(pinspotLightParent, "PinspotLight");
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
            //ストレートの場合
            if (angle == 0){
                    for (int i = 0; i < lights.Length; i++)
                    {
                        Light light = lights[i];
                        light.transform.rotation = Quaternion.Euler(-90f, light.transform.rotation.eulerAngles.y, light.transform.rotation.eulerAngles.z);
                    }
            //クロスの場合
            }else if (angle == 1){
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
            //放射の場合
            }else if (angle == 2){
                for (int i = 0; i < lights.Length; i++)
                {
                    Light light = lights[i];
                    light.transform.rotation = Quaternion.Euler(radialSearchLightRotations[i], light.transform.rotation.eulerAngles.y, light.transform.rotation.eulerAngles.z);
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

    void ArrangeAssets(int count, GameObject obj, GameObject parentObj, Vector3[] positions)
    {
        for (int i = 0; i < count; i++)
        {
            // 複製して位置を設定し、親を設定
            GameObject newAsset = Instantiate(obj, parentObj.transform);

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

    public void SaveLightDataFromButton()
    {
        SaveLightPositionAndAngle("SuspensionLight", suspensionLightParent);
        SaveLightRotationAndAngle("PinspotLight", pinspotLightParent);
    }

    public void SaveLightPositionAndAngle(string lightName, GameObject parentObj)
    {
        Light[] lights = parentObj.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            Light light = lights[i]; // Lightコンポーネントを取得

            PlayerPrefs.SetFloat(lightName + "PosX_" + i, light.transform.position.x);
            PlayerPrefs.SetFloat(lightName + "PosY_" + i, light.transform.position.y);
            PlayerPrefs.SetFloat(lightName + "PosZ_" + i, light.transform.position.z);
            PlayerPrefs.SetFloat(lightName + "Angle_" + i, light.spotAngle);
        }
        PlayerPrefs.Save();
    }

    public void SaveLightRotationAndAngle(string lightName, GameObject parentObj)
    {
        Light[] lights = parentObj.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            Light light = lights[i]; // Lightコンポーネントを取得

            // 回転をオイラー角として保存
            Vector3 rotationEuler = light.transform.rotation.eulerAngles;
            PlayerPrefs.SetFloat(lightName + "RotX_" + i, rotationEuler.x);
            PlayerPrefs.SetFloat(lightName + "RotY_" + i, rotationEuler.y);
            PlayerPrefs.SetFloat(lightName + "RotZ_" + i, rotationEuler.z);
            PlayerPrefs.SetFloat(lightName + "Angle_" + i, light.spotAngle);
        }
        PlayerPrefs.Save();
    }

    public void LoadLightPositionAndAngle(string lightName, GameObject parentObj)
    {
        Light[] lights = parentObj.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            Light light = lights[i]; // Lightコンポーネントを取得
            if (PlayerPrefs.HasKey(lightName + "PosX_" + i))
            {
                float x = PlayerPrefs.GetFloat(lightName + "PosX_" + i);
                float y = PlayerPrefs.GetFloat(lightName + "PosY_" + i);
                float z = PlayerPrefs.GetFloat(lightName + "PosZ_" + i);

                light.transform.position = new Vector3(x, y, z); // ポジションを設定
                light.spotAngle = PlayerPrefs.GetFloat(lightName + "Angle_" + i); // スポット角度を設定
            }
        }
    }

    public void LoadLightRotationAndAngle(string lightName, GameObject parentObj)
    {
        Light[] lights = parentObj.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            Light light = lights[i]; // Lightコンポーネントを取得
            if (PlayerPrefs.HasKey(lightName + "RotX_" + i))
            {
                float x = PlayerPrefs.GetFloat(lightName + "RotX_" + i);
                float y = PlayerPrefs.GetFloat(lightName + "RotY_" + i);
                float z = PlayerPrefs.GetFloat(lightName + "RotZ_" + i);

                // オイラー角をQuaternionに変換して回転を設定
                light.transform.rotation = Quaternion.Euler(x, y, z);
                light.spotAngle = PlayerPrefs.GetFloat(lightName + "Angle_" + i); // スポット角度を設定
            }
        }
    }
}
