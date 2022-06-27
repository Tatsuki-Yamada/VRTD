using UnityEngine;

public class TowerManager : SingletonMonoBehaviour<TowerManager>
{
    // 生成するタワーのPrefab
    [SerializeField] GameObject towerPrefab;

    // タワー生成の際に付けるY軸のオフセット
    float buildOffsetY = 0.5f;


    /// <summary>
    /// 引数のタイル上にタワーを生成する関数
    /// </summary>
    /// <param name="tilePos"></param>
    public void CreateTower(Vector3 tilePos)
    {
        Instantiate(towerPrefab, new Vector3(tilePos.x, tilePos.y + buildOffsetY, tilePos.z), Quaternion.identity);

    }
}
