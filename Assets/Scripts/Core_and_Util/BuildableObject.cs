using UnityEngine;
using TMPro;

/// <summary>
/// 建設・改造可能なオブジェクトの基底クラス
/// </summary>
public class BuildableObject : MonoBehaviour
{
    // 残りの叩く回数を示すテキスト
    [SerializeField] protected TextMeshPro buildCounter;

    // 良く呼ぶクラスの一次保存変数
    public BuildCounterController bcc;

    // 建設・改造中かを示すフラグ
    protected bool isBuilding = false;


    public virtual void Awake()
    {
        bcc = buildCounter.gameObject.GetComponent<BuildCounterController>();
    }


    /// <summary>
    /// 建設を開始するフラグをセットする関数
    /// </summary>
    public virtual void StartBuild() 
    {
        if (!isBuilding)
            isBuilding = true;
    }


    /// <summary>
    /// 建設を終了するフラグをセットする関数
    /// </summary>
    public virtual void CompleteBuild() 
    {
        isBuilding = false;
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
