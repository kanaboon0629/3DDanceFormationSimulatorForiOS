using UnityEngine;
using UnityEngine.UI; // UnityEngine.UIを追加

public class LightControlWithJoystick : MonoBehaviour
{
    private float moveSpeed = 5f; // 照明の移動速度
    private float rotationSpeed = 5f; // 照明の回転速度
    public Joystick joystick; // ジョイスティックの参照
    public Slider slider; // スライダーをSlider型に変更
    public GameObject susPanel;
    public GameObject pinPanel;

    public GameObject suspensionLightParent; // 照明オブジェクトの親
    public GameObject pinspotLightParent; // 照明オブジェクトの親

    private Light suspensionLightSource; // 操作する照明オブジェクト
    private Light pinspotLightSource; // 操作する照明オブジェクト
    private LightButtonsController lightButtonsController; // LightButtonsControllerのインスタンス

    void Start()
    {
        // LightButtonsControllerのインスタンスを取得
        lightButtonsController = FindObjectOfType<LightButtonsController>();
        if (lightButtonsController == null)
        {
            Debug.LogError("LightButtonsControllerが見つかりません。");
            return;
        }

        // スライダーの初期値を設定
        slider.onValueChanged.AddListener(OnSliderValueChanged); // スライダーの値が変わったときに呼び出すメソッドを登録
        
        int neededSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Toggle");
        int neededPinspotLight = PlayerPrefs.GetInt("PinspotLight_Toggle");
        // 最初の照明ソースを設定
        if (neededSuspensionLight == 1)
        {
           InisiateLightSource(suspensionLightParent, ref suspensionLightSource);
        }
        if (neededPinspotLight == 1)
        {
            InisiateLightSource(pinspotLightParent, ref pinspotLightSource);
        }
        // スライダーの値を現在のlightSourceのスポット角度に設定
        if (neededSuspensionLight == 1)
        {
            slider.value = suspensionLightSource.spotAngle; // スライダーに現在のスポット角度を設定
        }else{
            slider.value = pinspotLightSource.spotAngle;
        }
    }

    void Update()
    {
        if (susPanel.activeSelf)
        {
            SuspensionLightControlWithJoystick(suspensionLightSource);
        }else
        {
            PinspotLightControlWithJoystick(pinspotLightSource);
        }
        
    }
    public void SuspensionLightControlWithJoystick(Light lightSource)
    {
        if (lightSource != null) // suspensionlightSourceがnullでないことを確認
        {
            // ジョイスティックの入力を取得
            Vector2 input = joystick.GetInput();
            float horizontal = input.x; // ジョイスティックの横方向の入力
            float vertical = input.y; // ジョイスティックの縦方向の入力

            // ジョイスティックの入力に基づいて照明の移動方向を計算
            Vector3 moveDirection = new Vector3(-vertical, 0, horizontal); // X-Z平面での移動
                
            // 指定された照明オブジェクトの位置を移動
            lightSource.transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); // 照明を移動
        }
        else
        {
            Debug.LogWarning("現在のサス照明ソースが設定されていません。");
        }
    }
    public void PinspotLightControlWithJoystick(Light lightSource)
    {
        if (lightSource != null) // lightSourceがnullでないことを確認
        {
            // ジョイスティックの入力を取得
            Vector2 input = joystick.GetInput();
            float horizontal = input.x; // ジョイスティックの横方向の入力
            float vertical = input.y; // ジョイスティックの縦方向の入力

            // ジョイスティックの入力に基づいて照明の回転を計算
            Vector3 rotationDirection = new Vector3(-vertical, horizontal, 0); // 縦方向（x軸）、横方向（y軸）に回転
                    
            // 指定された照明オブジェクトの角度を変更
            lightSource.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime, Space.Self); // 照明を回転
        }
        else
        {
            Debug.LogWarning("現在のピンスポ照明ソースが設定されていません。");
        }
    }

    // スライダーの値が変わったときに呼び出されるメソッド
    public void OnSliderValueChanged(float value)
    {
        if (susPanel.activeSelf)
        {
            if (suspensionLightSource != null)
            {
                suspensionLightSource.spotAngle = value; // スライダーの値をlightSourceのスポット角度に適用
            }
        }else{
            if (pinspotLightSource != null)
            {
                pinspotLightSource.spotAngle = value; // スライダーの値をlightSourceのスポット角度に適用
            }
        }
    }

    public void InisiateLightSource(GameObject lightParent, ref Light lightSource)
    {
        if (lightButtonsController != null)
        {
            // lightParentの子オブジェクトから照明を探す
            Light[] lights = lightParent.GetComponentsInChildren<Light>();

            if (0 < lights.Length) // 照明が見つかった場合
            {
                lightSource = lights[0]; // 0番目のライトを設定
            }
            else
            {
                Debug.LogWarning("指定されたライトが見つかりません。");
            }
        }
    }

    public void SwitchLightSourceFromButton(int index)
    {
        if (lightButtonsController != null)
        {
            if (susPanel.activeSelf){
                SwitchLightSource(index, suspensionLightParent, ref suspensionLightSource);
            }else{
                SwitchLightSource(index, pinspotLightParent, ref pinspotLightSource);
            }
        }
    }

    public void SwitchLightSource(int index, GameObject lightParent, ref Light lightSource)
    {
        // suspensionLightParentの子オブジェクトから照明を探す
        Light[] lights = lightParent.GetComponentsInChildren<Light>();
        if (index < lights.Length) // インデックスが範囲内か確認
        {
            lightSource = lights[index]; // 指定されたインデックスのライトを設定

            // スライダーの値を現在のsuspensionlightSourceのスポット角度に設定
            slider.value = lightSource.spotAngle; // スライダーに現在のスポット角度を設定
        }
        else
        {
            Debug.LogWarning("指定されたインデックスのライトが見つかりません。");
        }
    }

    public float NowLightAngle()
    {
        if (susPanel.activeSelf)
        {
            if (suspensionLightSource != null)
            {
                return suspensionLightSource.spotAngle;
            }
        }
        else
        {
            if (pinspotLightSource != null)
            {
                return pinspotLightSource.spotAngle;
            }
        }

        // デフォルトで0を返す（ライトがnullの場合など）
        return 0f;
    }
}