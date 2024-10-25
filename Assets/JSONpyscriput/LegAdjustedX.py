import json

# Function to calculate the average of the second element (Y positions) of specified body parts
def calculate_average_y(data, parts):
    y_positions = [data[key][part][1] for key in data for part in parts]
    return sum(y_positions) / len(y_positions)

# Function to adjust the Y position of specified body parts equally
def adjust_positions(data, parts, adjustment_value):
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
    adjusted_data = adjust_positions(data, ["right_foot", "left_foot"], adjustment_value_foot)

    # Calculate the current average of the lower leg Y positions
    current_average_lowerleg = calculate_average_y(data, ["right_lowerleg", "left_lowerleg"])
    # Calculate the adjustment needed for lower leg positions
    adjustment_value_lowerleg = target_average_lowerleg - current_average_lowerleg
    # Adjust lower leg positions
    adjusted_data = adjust_positions(adjusted_data, ["right_lowerleg", "left_lowerleg"], adjustment_value_lowerleg)

    # Save the modified data back to the original JSON file
    with open(json_file_path, 'w') as outfile:
        json.dump(adjusted_data, outfile, indent=4)
    
    print(f"Adjustment complete! The modified data is saved to {json_file_path}")

# Example usage: 
json_file_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets/mikoniko.json'
target_average_foot = 0.02  # Desired average value for the Y positions of feet
target_average_lowerleg = 0.475  # Desired average value for the Y positions of lower legs

adjust_y_to_target(json_file_path, target_average_foot, target_average_lowerleg)

