using UnityEngine;
using UnityEngine.UI;
public class SampleSaveBool : MonoBehaviour
{
    public void isSample()
    {
        int number = 1;
        PlayerPrefs.SetInt("IsSample", number);
        PlayerPrefs.Save(); // 保存
    }
    public void isNotSample()
    {
        int number = 0;
        PlayerPrefs.SetInt("IsSample", number);
        PlayerPrefs.Save(); // 保存
    }
}