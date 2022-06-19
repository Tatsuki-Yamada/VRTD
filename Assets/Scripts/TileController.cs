using UnityEngine;
using TMPro;

public class TileController : MonoBehaviour
{
    [SerializeField] TextMeshPro buildCounter;
    BuildCounterController bcc;
    [SerializeField] Material[] tileMaterials = new Material[5];

    bool isBuilding = false;

    private void Awake()
    {
        bcc = buildCounter.gameObject.GetComponent<BuildCounterController>();
    }


    /// <summary>
    /// �^�C���̍��W�ɃI�t�Z�b�g���������l��Ԃ��֐��B
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos()
    {
        Vector3 offset = new Vector3(0, 0.2f, 0);
        return transform.position + offset;
    }


    public void StartBuild()
    {
        switch (tag)
        {
            case "Tile_None":
                bcc.SetCount(15);
                bcc.onCompleteBuild.AddListener(CompleteBuild);
                
                isBuilding = true;
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
    void CompleteBuild()
    {
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


    void OnTriggerEnter(Collider other)
    {
        if (!isBuilding)
            return;

        if (other.CompareTag("Hammer"))
        {
            buildCounter.gameObject.GetComponent<BuildCounterController>().DecreaseCount();
        }
    }
}
