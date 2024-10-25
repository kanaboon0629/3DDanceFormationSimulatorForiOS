import json  # Make sure to import the json module

# Script to read from a JSON file and calculate the averages of specified joint positions
def calculate_average_joint_x(json_file_path):
    # Load the JSON data from the file
    with open(json_file_path, 'r') as file:
        data = json.load(file)

    # Extract the X positions for the specified joints
    right_foot_x = [data[key]["right_foot"][0] for key in data]
    left_foot_x = [data[key]["left_foot"][0] for key in data]
    right_lowerleg_x = [data[key]["right_lowerleg"][0] for key in data]
    left_lowerleg_x = [data[key]["left_lowerleg"][0] for key in data]

    # Calculate the average for each joint
    average_right_foot_x = sum(right_foot_x) / len(right_foot_x) if right_foot_x else 0
    average_left_foot_x = sum(left_foot_x) / len(left_foot_x) if left_foot_x else 0
    average_right_lowerleg_x = sum(right_lowerleg_x) / len(right_lowerleg_x) if right_lowerleg_x else 0
    average_left_lowerleg_x = sum(left_lowerleg_x) / len(left_lowerleg_x) if left_lowerleg_x else 0

    # Calculate the overall average of the specified joints
    overall_average_x = (average_right_foot_x + average_left_foot_x + 
                         average_right_lowerleg_x + average_left_lowerleg_x) / 4

    return (average_right_foot_x, average_left_foot_x, 
            average_right_lowerleg_x, average_left_lowerleg_x, 
            overall_average_x)

# Example usage: replace 'your_file.json' with the actual path to the JSON file
json_file_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets/jazz.json'
averages = calculate_average_joint_x(json_file_path)

# Print the results
print(f"Average Right Foot X: {averages[0]}")
print(f"Average Left Foot X: {averages[1]}")
print(f"Average Right Lower Leg X: {averages[2]}")
print(f"Average Left Lower Leg X: {averages[3]}")
print(f"Overall Average X: {averages[4]}")
