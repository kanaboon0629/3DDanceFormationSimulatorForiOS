import json
import os

# Function to calculate the average of the second element of "hips"
def calculate_average_hip_y(data):
    hip_y_positions = [data[key]["hips"][1] for key in data]
    return sum(hip_y_positions) / len(hip_y_positions)

# Function to adjust the second element of all body parts equally
def adjust_body_part_positions(data, adjustment_value):
    for key in data:
        for part in data[key]:
            data[key][part][1] += adjustment_value  # Adjust the second element equally for all parts
    return data

# Main function
def adjust_hip_y_to_target_in_directory(directory_path, target_average):
    # Iterate over all files in the directory
    for filename in os.listdir(directory_path):
        if filename.endswith('.json'):
            json_file_path = os.path.join(directory_path, filename)
            
            # Load the JSON data from the file
            with open(json_file_path, 'r') as file:
                data = json.load(file)

            # Calculate the current average of the "hips" second element
            current_average = calculate_average_hip_y(data)

            # Calculate the adjustment needed to reach the target average
            adjustment_value = target_average - current_average

            # Adjust all body parts by the calculated value
            adjusted_data = adjust_body_part_positions(data, adjustment_value)

            # Save the modified data back to the original JSON file
            with open(json_file_path, 'w') as outfile:
                json.dump(adjusted_data, outfile, indent=4)

            print(f"Adjustment complete for {filename}! The modified data is saved to {json_file_path}")

# Example usage: 
directory_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets'
target_average = 0.89  # Desired average value for the second element of "hips"

adjust_hip_y_to_target_in_directory(directory_path, target_average)