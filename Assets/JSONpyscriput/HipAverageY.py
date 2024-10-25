import json  # Make sure to import the json module

# Script to read from a JSON file and calculate the average of the second element of "hips"
def calculate_average_hip_y(json_file_path):
    # Load the JSON data from the file
    with open(json_file_path, 'r') as file:
        data = json.load(file)

    # Extract the second element of "hips" for all entries
    hip_y_positions = [data[key]["hips"][1] for key in data]

    # Calculate the average of the second elements (y positions)
    average_hip_y = sum(hip_y_positions) / len(hip_y_positions)
    
    return average_hip_y

# Example usage: replace 'your_file.json' with the actual path to the JSON file
# json_file_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets/hula.json'
json_file_path = '/Users/kanakokunii/3DDanceFormationSimulatorForiOS/Assets/StreamingAssets/misamo.json'
average_hip_y = calculate_average_hip_y(json_file_path)
print(average_hip_y)
