import json
import os

# Function to calculate the average of the second element (Y positions) of specified body parts
def calculate_average_y(data, parts):
    y_positions = [data[key][part][1] for key in data for part in parts]
    return sum(y_positions) / len(y_positions)

# Function to adjust the Y position of specified body parts equally
def adjust_positions_y(data, parts, adjustment_value):
    for key in data:
        for part in parts:
            data[key][part][1] += adjustment_value
    return data

# Main function to adjust foot and lower leg Y positions to target average
def adjust_y_to_target(json_file_path, target_average_foot, target_average_lowerleg):
    # Load the JSON data from the file
    with open(json_file_path, 'r') as file:
        data = json.load(file)

    # Calculate the current average of the foot Y positions
    current_average_foot = calculate_average_y(data, ["right_foot", "left_foot"])
    # Calculate the adjustment needed for foot positions
    adjustment_value_foot = target_average_foot - current_average_foot
    # Adjust foot positions
    adjusted_data = adjust_positions_y(data, ["right_foot", "left_foot"], adjustment_value_foot)

    # Calculate the current average of the lower leg Y positions
    current_average_lowerleg = calculate_average_y(data, ["right_lowerleg", "left_lowerleg"])
    # Calculate the adjustment needed for lower leg positions
    adjustment_value_lowerleg = target_average_lowerleg - current_average_lowerleg
    # Adjust lower leg positions
    adjusted_data = adjust_positions_y(adjusted_data, ["right_lowerleg", "left_lowerleg"], adjustment_value_lowerleg)

    # Save the modified data back to the original JSON file
    with open(json_file_path, 'w') as outfile:
        json.dump(adjusted_data, outfile, indent=4)
    
    print(f"Y adjustment complete! The modified data is saved to {json_file_path}")

# Function to calculate the average of the first element (X positions) of specified body parts
def calculate_average_x(data, parts):
    x_positions = [data[key][part][0] for key in data for part in parts]
    return sum(x_positions) / len(x_positions)

# Function to adjust the X position of specified body parts equally
def adjust_positions_x(data, parts, adjustment_value):
    for key in data:
        for part in parts:
            data[key][part][0] += adjustment_value
    return data

# Main function to adjust foot and lower leg X positions to target average
def adjust_x_to_target(json_file_path, target_average_foot, target_average_lowerleg):
    # Load the JSON data from the file
    with open(json_file_path, 'r') as file:
        data = json.load(file)

    # Calculate the current average of the foot X positions
    current_average_foot = calculate_average_x(data, ["right_foot", "left_foot"])
    # Calculate the adjustment needed for foot positions
    adjustment_value_foot = target_average_foot - current_average_foot
    # Adjust foot positions
    adjusted_data = adjust_positions_x(data, ["right_foot", "left_foot"], adjustment_value_foot)

    # Calculate the current average of the lower leg X positions
    current_average_lowerleg = calculate_average_x(data, ["right_lowerleg", "left_lowerleg"])
    # Calculate the adjustment needed for lower leg positions
    adjustment_value_lowerleg = target_average_lowerleg - current_average_lowerleg
    # Adjust lower leg positions
    adjusted_data = adjust_positions_x(adjusted_data, ["right_lowerleg", "left_lowerleg"], adjustment_value_lowerleg)

    # Save the modified data back to the original JSON file
    with open(json_file_path, 'w') as outfile:
        json.dump(adjusted_data, outfile, indent=4)
    
    print(f"X adjustment complete! The modified data is saved to {json_file_path}")

# Function to process all JSON files in a given directory
def process_all_json_files(directory, target_average_foot_y, target_average_lowerleg_y, target_average_foot_x, target_average_lowerleg_x):
    for filename in os.listdir(directory):
        if filename.endswith('.json'):
            json_file_path = os.path.join(directory, filename)
            print(f"Processing {json_file_path}...")
            adjust_y_to_target(json_file_path, target_average_foot_y, target_average_lowerleg_y)
            adjust_x_to_target(json_file_path, target_average_foot_x, target_average_lowerleg_x)

# Example usage:
directory_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets/'  # Directory containing JSON files
target_average_foot_y = 0.02  # Desired average value for the Y positions of feet
target_average_lowerleg_y = 0.475  # Desired average value for the Y positions of lower legs
target_average_foot_x = 0.03  # Desired average value for the X positions of feet
target_average_lowerleg_x = 0.03  # Desired average value for the X positions of lower legs

process_all_json_files(directory_path, target_average_foot_y, target_average_lowerleg_y, target_average_foot_x, target_average_lowerleg_x)
