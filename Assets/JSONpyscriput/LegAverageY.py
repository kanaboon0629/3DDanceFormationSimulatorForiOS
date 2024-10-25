import json  # Make sure to import the json module

# Script to read from a JSON file and calculate the averages of specified joint positions
def calculate_average_joint_y(json_file_path):
    # Load the JSON data from the file
    with open(json_file_path, 'r') as file:
        data = json.load(file)

    # Extract the Y positions for the specified joints
    right_foot_y = [data[key]["right_foot"][1] for key in data]
    left_foot_y = [data[key]["left_foot"][1] for key in data]
    right_lowerleg_y = [data[key]["right_lowerleg"][1] for key in data]
    left_lowerleg_y = [data[key]["left_lowerleg"][1] for key in data]

    # Calculate the average for each joint
    average_right_foot_y = sum(right_foot_y) / len(right_foot_y) if right_foot_y else 0
    average_left_foot_y = sum(left_foot_y) / len(left_foot_y) if left_foot_y else 0
    average_right_lowerleg_y = sum(right_lowerleg_y) / len(right_lowerleg_y) if right_lowerleg_y else 0
    average_left_lowerleg_y = sum(left_lowerleg_y) / len(left_lowerleg_y) if left_lowerleg_y else 0

    return (average_right_foot_y, average_left_foot_y, 
            average_right_lowerleg_y, average_left_lowerleg_y, 
            )

# Example usage: replace 'your_file.json' with the actual path to the JSON file
json_file_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets/hula.json'
averages = calculate_average_joint_y(json_file_path)

# Print the results
print(f"Average Right Foot Y: {averages[0]}")
print(f"Average Left Foot Y: {averages[1]}")
print(f"Average Right Lower Leg Y: {averages[2]}")
print(f"Average Left Lower Leg Y: {averages[3]}")

