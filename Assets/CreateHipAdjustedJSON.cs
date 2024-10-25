using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class CreateHipAdjustedJSON
{
    // Function to calculate the average of the second element of "hips"
    static double CalculateAverageHipY(Dictionary<string, Dictionary<string, List<double>>> data)
    {
        double sum = 0;
        int count = 0;

        foreach (var key in data.Keys)
        {
            sum += data[key]["hips"][1];  // Get the second element (Y coordinate) of "hips"
            count++;
        }

        return sum / count;
    }

    // Function to adjust the second element (Y coordinate) of all body parts equally
    static void AdjustBodyPartPositions(Dictionary<string, Dictionary<string, List<double>>> data, double adjustmentValue)
    {
        foreach (var key in data.Keys)
        {
            foreach (var part in data[key].Keys)
            {
                data[key][part][1] += adjustmentValue;  // Adjust the second element (Y coordinate)
            }
        }
    }

    // Main function
    static void AdjustHipYToTarget(string jsonFilePath, double targetAverage)
    {
        // Load the JSON data from the file
        string jsonData = File.ReadAllText(jsonFilePath);
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<double>>>>(jsonData);

        // Calculate the current average of the "hips" second element (Y coordinate)
        double currentAverage = CalculateAverageHipY(data);

        // Calculate the adjustment needed to reach the target average
        double adjustmentValue = targetAverage - currentAverage;

        // Adjust all body parts by the calculated value
        AdjustBodyPartPositions(data, adjustmentValue);

        // Save the modified data back to the same JSON file
        string adjustedJsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(jsonFilePath, adjustedJsonData);

        Console.WriteLine($"Adjustment complete! The modified data has been saved to {jsonFilePath}");
    }

    // Example usage
    public static void Adjust(string jsonFilePath)
    {
        double targetAverage = 0.89;  // Desired average value for the second element of "hips"

        AdjustHipYToTarget(jsonFilePath, targetAverage);
    }
}