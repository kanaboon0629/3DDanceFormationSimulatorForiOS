using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    public bool isPortrait;

    void Start()
    {
        if (isPortrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
