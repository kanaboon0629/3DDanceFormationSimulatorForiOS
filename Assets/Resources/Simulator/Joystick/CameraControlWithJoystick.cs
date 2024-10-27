using UnityEngine;

public class CameraControlWithJoystick : MonoBehaviour
{
    public Joystick joystick;
    private float rotationSpeed = 0.5f;

    void Update()
    {
        Vector2 input = joystick.GetInput();
        Vector3 rotation = new Vector3(-input.y, input.x, 0) * rotationSpeed;
        transform.eulerAngles += rotation;
    }
}
