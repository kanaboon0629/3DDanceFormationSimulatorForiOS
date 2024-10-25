using UnityEngine;

public class ObjectDataLoader : MonoBehaviour
{
    public GameObject assetPrefab;  // ロードする際に使用するプレハブ
    private Vector3[] positions = {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 2),
        new Vector3(0, 0, -2),
        new Vector3(0, 0, 4),
        new Vector3(0, 0, -4),
        new Vector3(0, 0, 6),
        new Vector3(0, 0, -6),
        new Vector3(0, 0, 8),
        new Vector3(0, 0, -8),
        new Vector3(0, 0, 10),
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, 2),
        new Vector3(-1, 0, -2),
        new Vector3(-1, 0, 4),
        new Vector3(-1, 0, -4),
        new Vector3(-1, 0, 6),
        new Vector3(-1, 0, -6),
        new Vector3(-1, 0, 8),
        new Vector3(-1, 0, -8),
        new Vector3(-1, 0, 10)
    };

    void Start()
    {
        LoadObjectData();
    }

    void LoadObjectData()
    {
        int objectCount = PlayerPrefs.GetInt("ObjectCount", 0);  // 保存されたオブジェクトの数を取得

        for (int i = 0; i < objectCount; i++)
        {
            GameObject newObj = Instantiate(assetPrefab);

            // 位置の復元、保存されていない場合は positions[i] を使用
            float posX = PlayerPrefs.GetFloat("ObjectPosX_" + i, float.NaN);
            float posY = PlayerPrefs.GetFloat("ObjectPosY_" + i, float.NaN);
            float posZ = PlayerPrefs.GetFloat("ObjectPosZ_" + i, float.NaN);

            if (float.IsNaN(posX) || float.IsNaN(posY) || float.IsNaN(posZ))
            {
                // 保存されていない場合、positions[i] を使用
                newObj.transform.position = positions[i];
            }
            else
            {
                // 保存されている場合、その値を使用
                newObj.transform.position = new Vector3(posX, posY, posZ);
            }

            // Rendererコンポーネントを取得
            Renderer renderer = newObj.GetComponent<Renderer>();

            // 色の復元
            float colorR = PlayerPrefs.GetFloat("ObjectColorR_" + i, 1);
            float colorG = PlayerPrefs.GetFloat("ObjectColorG_" + i, 1);
            float colorB = PlayerPrefs.GetFloat("ObjectColorB_" + i, 1);
            Color restoredColor = new Color(colorR, colorG, colorB);

            // Rendererコンポーネントに対して色を適用
            if (renderer != null && renderer.material != null)
            {
                renderer.material.color = restoredColor;
            }
            else
            {
                Debug.LogWarning("Renderer または Material が見つかりませんでした。");
            }
            newObj.transform.rotation = Quaternion.Euler(0, 90, 0); // y軸を90度回転
        }
    }
}
