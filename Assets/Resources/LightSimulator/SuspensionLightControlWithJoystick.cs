using UnityEngine;
using UnityEngine.UI; // UnityEngine.UIを追加

public class SuspensionLightControlWithJoystick : MonoBehaviour
{
    public float moveSpeed = 5f; // 照明の移動速度
    public Joystick joystick; // ジョイスティックの参照
    public Slider slider; // スライダーをSlider型に変更

    public GameObject suspensionLightParent; // 照明オブジェクトの親

    private Light lightSource; // 操作する照明オブジェクト
    private SuspensionLightController suspensionLightController; // SuspensionLightControllerのインスタンス

    void Start()
    {
        // SuspensionLightControllerのインスタンスを取得
        suspensionLightController = FindObjectOfType<SuspensionLightController>();
        if (suspensionLightController == null)
        {
            Debug.LogError("SuspensionLightControllerが見つかりません。");
            return;
        }

        // スライダーの初期値を設定
        slider.value = 30f; // 例: スポット角度の初期値を30度に設定
        slider.onValueChanged.AddListener(OnSliderValueChanged); // スライダーの値が変わったときに呼び出すメソッドを登録

        // 最初の照明ソースを設定
        SwitchLightSource(0); // 0番目のライトを選択
    }

    void Update()
    {
        if (lightSource != null) // lightSourceがnullでないことを確認
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
            Debug.LogWarning("現在の照明ソースが設定されていません。");
        }
    }

    public void SwitchLightSource(int index)
    {
        if (suspensionLightController != null)
        {
            // suspensionLightParentの子オブジェクトから照明を探す
            Light[] lights = suspensionLightParent.GetComponentsInChildren<Light>();

            if (index < lights.Length) // インデックスが範囲内か確認
            {
                lightSource = lights[index]; // 指定されたインデックスのライトを設定

                // スライダーの値を現在のlightSourceのスポット角度に設定
                slider.value = lightSource.spotAngle; // スライダーに現在のスポット角度を設定
            }
            else
            {
                Debug.LogWarning("指定されたインデックスのライトが見つかりません。");
            }
        }
    }

    // スライダーの値が変わったときに呼び出されるメソッド
    public void OnSliderValueChanged(float value)
    {
        if (lightSource != null)
        {
            lightSource.spotAngle = value; // スライダーの値をlightSourceのスポット角度に適用
        }
    }
}