using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;

public class TestJson : MonoBehaviour
{
    void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "output.json");
        //jsonを編集する、対象を作成
        AdjustedJSONFile(jsonFilePath);
    }

    private void ProcessJsonFile(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        Debug.Log($"JSON file created at: : {filePath}");
    }

    private void CreateSymmetryFile(string inputFilePath)
    {
        string outputFilePath = inputFilePath.Replace(".json", "Symmetry.json");

        CreateSymmetryJSON.ProcessJson(inputFilePath, outputFilePath);

        Debug.Log($"Symmetry JSON file created at: {outputFilePath}");
    }

    private void CreateHipAdjustedFile(string inputFilePath)
    {
        CreateHipAdjustedJSON.Adjust(inputFilePath);

        Debug.Log($"ALL from Hip Adjusted JSON file at: {inputFilePath}");
    }
    private void CreateLegAdjustedFile(string inputFilePath)
    {
        CreateRegAdjustedJSON.Adjust(inputFilePath);

        Debug.Log($"Reg Adjusted JSON file at: {inputFilePath}");
    }

    public void AdjustedJSONFile(string jsonFilePath){
        CreateHipAdjustedFile(jsonFilePath);
        CreateLegAdjustedFile(jsonFilePath);
        CreateSymmetryFile(jsonFilePath);
        Debug.Log("JSON作成完了");
    }
}