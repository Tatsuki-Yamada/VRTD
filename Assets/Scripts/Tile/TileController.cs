using UnityEngine;

public class TileController : MonoBehaviour
{
    // GetPosの座標を変えるための半タイルのタグリスト
    string[] halfTileTags_toChangeOffsetY = { "Tile_Road", "Tile_EnemyBase", "Tile_PlayerBase" };

    // プレイヤーが移動可能なタイルかのフラグ
    public bool isPlayerMovable { get; private set; } = true;

    bool isBuilding_toAvoidMultiBuildIng = false;


    /// <summary>
    /// タイルの表面の座標を返す関数
    /// </summary>
    /// <returns></returns>
    /// TODO. offsetYの意味を調べる
    public Vector3 GetSurfacePos(float tileHeight_toAddPos = 0.2f)
    {
        if (Utils.CompareTags(tag, halfTileTags_toChangeOffsetY))
            tileHeight_toAddPos -= 0.1f;

        Vector3 offset = new Vector3(0, tileHeight_toAddPos, 0);
        return transform.position + offset;
    }


    public void StartBuild()
    {
        if (isBuilding_toAvoidMultiBuildIng)
            return;

        switch (tag)
        {
            case "Tile_None":
                // 座標の追加分はConstructioniSiteの足をタイルの表面につけるため。
                ConstructionSiteController tempCSC_toCallSometime = ConstructionManager.Instance.CreateConstructionSite(GetSurfacePos() + new Vector3(0, 0.25f, 0), 25);
                tempCSC_toCallSometime.onCompleteBuildFuncs_toCallback.AddListener(CompleteBuild);
                break;

                /*
                case "Tile_CanBuild":
                    bcc.SetCount(25);
                    bcc.onCompleteBuildFuncs_toCallback.AddListener(CompleteBuild);
                    ConstructionManager.Instance.CreateConstructionSite(GetPos(), bcc, 25);
                    break;
                */
        }

        isPlayerMovable = false;
        isBuilding_toAvoidMultiBuildIng = true;
    }


    /// <summary>
    /// カウンターが0になった時、コールバックされる関数。
    /// </summary>
    public void CompleteBuild()
    {
        switch (tag)
        {
            case "Tile_None":
                TowerManager.Instance.CreateTower(transform.position);
                break;

                /*
                case "Tile_None":
                    transform.GetChild(0).GetComponent<Renderer>().material = tileMaterials[2];
                    tag = "Tile_CanBuild";
                    isMovable = true;
                    break;
                */
        }

        isBuilding_toAvoidMultiBuildIng = false;
    }
}
