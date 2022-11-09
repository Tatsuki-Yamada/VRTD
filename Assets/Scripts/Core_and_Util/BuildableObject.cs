using UnityEngine;
using TMPro;

/// <summary>
/// 建設・改造可能なオブジェクトの基底クラス
/// </summary>
public class BuildableObject : MonoBehaviour
{
    // 建設・改造中かを示すフラグ
    protected bool isBuilding = false;


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

}
