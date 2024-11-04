using UnityEngine;

public class CameraControlWithJoystick : MonoBehaviour
{
    public Joystick joystick;
    private float rotationSpeed = 0.5f;

    void Start()
    {
        LoadCameraSettings();
    }

    void Update()
    {
        Vector2 input = joystick.GetInput();
        Vector3 rotation = new Vector3(-input.y, input.x, 0) * rotationSpeed;
        transform.eulerAngles += rotation;
    }

    // カメラ設定を保存
    public void SaveCameraSettings()
    {
        PlayerPrefs.SetFloat("CameraRotX", transform.eulerAngles.x);
        PlayerPrefs.SetFloat("CameraRotY", transform.eulerAngles.y);
        PlayerPrefs.SetFloat("CameraRotZ", transform.eulerAngles.z);
        PlayerPrefs.Save();
    }

    // カメラ設定を読み込む
    private void LoadCameraSettings()
    {
        if (PlayerPrefs.HasKey("CameraRotX"))
        {
            float rotX = PlayerPrefs.GetFloat("CameraRotX");
            float rotY = PlayerPrefs.GetFloat("CameraRotY");
            float rotZ = PlayerPrefs.GetFloat("CameraRotZ");
            transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
        }
    }
}
