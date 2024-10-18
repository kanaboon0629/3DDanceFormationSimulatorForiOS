using UnityEngine;
using UnityEngine.UI;
using System.IO; // ファイル操作のために必要

public class ButtonUIController : MonoBehaviour
{
    void Start()
    {
        // "output.json" ファイルが Application.persistentDataPath に存在するか確認
        string filePath = Path.Combine(Application.persistentDataPath, "output.json");
        bool fileExists = File.Exists(filePath);
        
        // ファイルがあったらオブジェクトを表示、なければ非表示
        gameObject.SetActive(fileExists);
    }
}