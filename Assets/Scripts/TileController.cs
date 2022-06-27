using UnityEngine;
using TMPro;

public class TileController : BuildableObject
{
    // タイルが変化する際に付けるマテリアルのリスト
    [SerializeField] Material[] tileMaterials = new Material[5];

    string[] halfTileTags = { "Tile_Road", "Tile_EnemyBase", "Tile_PlayerBase" };


    /// <summary>
    /// タイルの座標にオフセットを加えた値を返す関数。
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos(float offsetY = 0.2f)
    {
        if (Utils.CompareTags(tag, halfTileTags))
            offsetY -= 0.1f;

        Vector3 offset = new Vector3(0, offsetY, 0);
        return transform.position + offset;
    }


    public override void StartBuild()
    {
        if (isBuilding)
            return;

        base.StartBuild();

        switch (tag)
        {
            case "Tile_None":
                bcc.SetCount(15);
                bcc.onCompleteBuild.AddListener(CompleteBuild);
                break;

            case "Tile_CanBuild":
                bcc.SetCount(25);
                bcc.onCompleteBuild.AddListener(CompleteBuild);
                break;
        }
    }


    /// <summary>
    /// カウンターが0になった時、コールバックされる関数。
    /// </summary>
    public override void CompleteBuild()
    {
        base.CompleteBuild();

        switch (tag)
        {
            case "Tile_None":
                transform.GetChild(0).GetComponent<Renderer>().material = tileMaterials[2];
                tag = "Tile_CanBuild";
                break;

            case "Tile_CanBuild":
                TowerManager.Instance.CreateTower(transform.position);
                tag = "Tile_Built";
                break;
        }

    }


}
