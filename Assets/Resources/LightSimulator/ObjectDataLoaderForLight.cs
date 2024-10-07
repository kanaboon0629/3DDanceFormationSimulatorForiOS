using UnityEngine;

public class ObjectDataLoaderForLight : MonoBehaviour
{
    public GameObject assetPrefab;  // ロードする際に使用するプレハブ

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

            // 位置の復元
            float posX = PlayerPrefs.GetFloat("ObjectPosX_" + i, 0);
            float posY = PlayerPrefs.GetFloat("ObjectPosY_" + i, 0);
            float posZ = PlayerPrefs.GetFloat("ObjectPosZ_" + i, 0);
            newObj.transform.position = new Vector3(posX, posY, posZ);

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
