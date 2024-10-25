import os
import json

class CreateSymmetryJSON:
    @staticmethod
    def process_json(input_file_path, output_file_path):
        if not os.path.exists(input_file_path):
            print(f"Input file not found: {input_file_path}")
            return

        with open(input_file_path, 'r') as file:
            json_data = file.read()

        try:
            data = json.loads(json_data)
            if data is None:
                print("Failed to parse input JSON data")
                return

            converted_data = {}
            for key in data.keys():
                inner_dict = data[key]
                if inner_dict is None:
                    print(f"Inner dictionary is null for key: {key}")
                    return

                new_inner_dict = {}
                for inner_key in inner_dict.keys():
                    float_list = inner_dict[inner_key]
                    if float_list is None:
                        print(f"Float list is null for key: {inner_key}")
                        return

                    new_inner_dict[inner_key] = [float(item) for item in float_list]

                converted_data[key] = new_inner_dict

            for key in converted_data.keys():
                for joint in converted_data[key].keys():
                    converted_data[key][joint][2] = -converted_data[key][joint][2]

            for key in converted_data.keys():
                CreateSymmetryJSON.swap_values(converted_data[key], "left_upperleg", "right_upperleg")
                CreateSymmetryJSON.swap_values(converted_data[key], "left_lowerleg", "right_lowerleg")
                CreateSymmetryJSON.swap_values(converted_data[key], "left_foot", "right_foot")
                CreateSymmetryJSON.swap_values(converted_data[key], "left_upperarm", "right_upperarm")
                CreateSymmetryJSON.swap_values(converted_data[key], "left_lowerarm", "right_lowerarm")
                CreateSymmetryJSON.swap_values(converted_data[key], "left_hand", "right_hand")

            new_json_data = json.dumps(converted_data, indent=4)
            with open(output_file_path, 'w') as file:
                file.write(new_json_data)

            print(f"新しいJSONファイルが作成されました: {output_file_path}")
        except Exception as ex:
            print(f"Exception during JSON processing: {ex}")

    @staticmethod
    def swap_values(data, key1, key2):
        if key1 in data and key2 in data:
            data[key1], data[key2] = data[key2], data[key1]

def process_all_json_in_folder(input_folder):
    for filename in os.listdir(input_folder):
        if filename.endswith('.json'):
            input_file_path = os.path.join(input_folder, filename)
            # 新しいファイル名を生成 (dance.json → danceSymmetry.json)
            new_filename = f"{os.path.splitext(filename)[0]}Symmetry.json"
            output_file_path = os.path.join(input_folder, new_filename)
            CreateSymmetryJSON.process_json(input_file_path, output_file_path)

input_folder = 'Assets/StreamingAssets'
process_all_json_in_folder(input_folder)
