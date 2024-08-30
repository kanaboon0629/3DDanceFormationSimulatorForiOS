using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Start()
    {
        // PlayerPrefsの全てのデータを削除
        PlayerPrefs.DeleteAll();

        // PlayerPrefsの変更をディスクに保存
        PlayerPrefs.Save();

        // コンソールに確認メッセージを表示
        Debug.Log("All PlayerPrefs have been reset on start.");
    }
}
