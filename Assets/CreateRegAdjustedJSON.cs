using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class CreateRegAdjustedJSON
{
    // Function to calculate the average of the Y positions of specified body parts
    static double CalculateAverageY(Dictionary<string, Dictionary<string, List<double>>> data, List<string> parts)
    {
        double sum = 0;
        int count = 0;

        foreach (var key in data.Keys)
        {
            foreach (var part in parts)
            {
                if (data[key].ContainsKey(part))
                {
                    var joint = data[key][part];
                    sum += joint[1]; // Y position
                    count++;
                }
            }
        }

        return count > 0 ? sum / count : 0;
    }

    // Function to calculate the average of the X positions of specified body parts
    static double CalculateAverageX(Dictionary<string, Dictionary<string, List<double>>> data, List<string> parts)
    {
        double sum = 0;
        int count = 0;

        foreach (var key in data.Keys)
        {
            foreach (var part in parts)
            {
                if (data[key].ContainsKey(part))
                {
                    var joint = data[key][part];
                    sum += joint[0]; // X position
                    count++;
                }
            }
        }

        return count > 0 ? sum / count : 0;
    }

    // Function to adjust the Y position of specified body parts
    static void AdjustPositionsY(Dictionary<string, Dictionary<string, List<double>>> data, List<string> parts, double adjustmentValue)
    {
        foreach (var key in data.Keys)
        {
            foreach (var part in parts)
            {
                if (data[key].ContainsKey(part))
                {
                    var joint = data[key][part];
                    joint[1] += adjustmentValue; // Adjust Y position
                }
            }
        }
    }

    // Function to adjust the X position of specified body parts
    static void AdjustPositionsX(Dictionary<string, Dictionary<string, List<double>>> data, List<string> parts, double adjustmentValue)
    {
        foreach (var key in data.Keys)
        {
            foreach (var part in parts)
            {
                if (data[key].ContainsKey(part))
                {
                    var joint = data[key][part];
                    joint[0] += adjustmentValue; // Adjust X position
                }
            }
        }
    }

    // Main function to adjust foot and lower leg positions to target averages
    static void AdjustPositionsToTarget(string jsonFilePath, double targetAverageFootY, double targetAverageLowerlegY, double targetAverageFootX, double targetAverageLowerlegX)
    {
        // Load the JSON data from the file
        var jsonData = File.ReadAllText(jsonFilePath);
        var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<double>>>>(jsonData);

        // Calculate the current average of the foot Y positions
        double currentAverageFootY = CalculateAverageY(data, new List<string> { "right_foot", "left_foot" });
        double adjustmentValueFootY = targetAverageFootY - currentAverageFootY;
        AdjustPositionsY(data, new List<string> { "right_foot", "left_foot" }, adjustmentValueFootY);

        // Calculate the current average of the lower leg Y positions
        double currentAverageLowerlegY = CalculateAverageY(data, new List<string> { "right_lowerleg", "left_lowerleg" });
        double adjustmentValueLowerlegY = targetAverageLowerlegY - currentAverageLowerlegY;
        AdjustPositionsY(data, new List<string> { "right_lowerleg", "left_lowerleg" }, adjustmentValueLowerlegY);

        // Calculate the current average of the foot X positions
        double currentAverageFootX = CalculateAverageX(data, new List<string> { "right_foot", "left_foot" });
        double adjustmentValueFootX = targetAverageFootX - currentAverageFootX;
        AdjustPositionsX(data, new List<string> { "right_foot", "left_foot" }, adjustmentValueFootX);

        // Calculate the current average of the lower leg X positions
        double currentAverageLowerlegX = CalculateAverageX(data, new List<string> { "right_lowerleg", "left_lowerleg" });
        double adjustmentValueLowerlegX = targetAverageLowerlegX - currentAverageLowerlegX;
        AdjustPositionsX(data, new List<string> { "right_lowerleg", "left_lowerleg" }, adjustmentValueLowerlegX);

        // Save the adjusted data back to the original file
        string modifiedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(jsonFilePath, modifiedJson);

        Console.WriteLine($"Adjustment complete! The modified data is saved to {jsonFilePath}");
    }

    // Example usage:
    public static void Adjust(string jsonFilePath)
    {
        double targetAverageFootY = 0.02;  // Desired average Y position for feet
        double targetAverageLowerlegY = 0.475;  // Desired average Y position for lower legs
        double targetAverageFootX = 0.03;  // Desired average X position for feet
        double targetAverageLowerlegX = 0.03;  // Desired average X position for lower legs

        AdjustPositionsToTarget(jsonFilePath, targetAverageFootY, targetAverageLowerlegY, targetAverageFootX, targetAverageLowerlegX);
    }
}
