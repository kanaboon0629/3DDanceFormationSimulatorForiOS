using UnityEngine;

public class ArmatureDataLoader : MonoBehaviour
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

            // すべての子オブジェクトのRendererコンポーネントを取得
            Renderer[] renderers = newObj.GetComponentsInChildren<Renderer>();

            // 反転かどうかの復元
            float colorR = PlayerPrefs.GetFloat("ObjectColorR_" + i, 1);
            float colorG = PlayerPrefs.GetFloat("ObjectColorG_" + i, 1);
            float colorB = PlayerPrefs.GetFloat("ObjectColorB_" + i, 1);
            Color restoredColor = new Color(colorR, colorG, colorB);

            // Color.redのケースを特別扱い
            if (restoredColor == Color.red)
            {
                // Color.redの場合はタグを付与して色は変更しない
                newObj.tag = "isSymmetry"; // 任意のタグを設定
            }

            // コスチューム色の復元
            float bodyColorR = PlayerPrefs.GetFloat("Body_Red", 1f);
            float bodyColorG = PlayerPrefs.GetFloat("Body_Green", 1f);
            float bodyColorB = PlayerPrefs.GetFloat("Body_Blue", 1f);
            Color bodyColor = new Color(bodyColorR, bodyColorG, bodyColorB);
            
            // 他の色の場合は通常通り色を適用
            foreach (Renderer renderer in renderers)
            {
                // 複数のマテリアルが存在する場合も考慮
                foreach (Material mat in renderer.materials)
                {
                    mat.color = bodyColor;
                }
            }            
        }
    }
}
