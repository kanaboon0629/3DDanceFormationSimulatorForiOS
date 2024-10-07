using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために必要

public class SceneSwitcher : MonoBehaviour
{
    public ObjectDataSaver dataSaver;  // データを保存するためのスクリプトを参照
    public static bool IsReturningFromNumberSetting = false;
    
    // 指定されたシーンに遷移するメソッド
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void SaveObjectDataBeforeSwitch(string sceneName)
    {
        // データを保存
        dataSaver.SaveObjectData();
    }

    // シーンを次に進めるメソッド（例：現在のシーンの次のシーンに遷移）
    public void SwitchToNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void BackButtonInNumberSetting()
    {
        int isSample = PlayerPrefs.GetInt("IsSample", 1);
        if (isSample == 1) {
            SwitchToScene("Title");
        }else{
            IsReturningFromNumberSetting = true;
            SwitchToScene("Estimation");
        }
    }
    //Suspensionlightがある場合
    public void NextButtonInLightSetting(){
        int neededSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Toggle");
        if (neededSuspensionLight == 1) {
            SwitchToScene("LightSimulator");
        }else{
            SwitchToScene("Simulator");
        }
    }
    public void BackButtonInSimulator()
    {
        int neededSuspensionLight = PlayerPrefs.GetInt("SuspensionLight_Toggle");
        if (neededSuspensionLight == 1) {
            SwitchToScene("LightSimulator");
        }else{
            SwitchToScene("LightSetting");
        }
    }
    
}
