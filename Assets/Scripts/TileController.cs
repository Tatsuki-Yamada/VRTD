using UnityEngine;
using TMPro;

public class TileController : BuildableObject
{
    [SerializeField] Material[] tileMaterials = new Material[5];


    /// <summary>
    /// �^�C���̍��W�ɃI�t�Z�b�g���������l��Ԃ��֐��B
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos()
    {
        Vector3 offset = new Vector3(0, 0.2f, 0);
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
    /// �J�E���^�[��0�ɂȂ������A�R�[���o�b�N�����֐��B
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
