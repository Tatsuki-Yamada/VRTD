using UnityEngine;

public class EnergyFieldController : MonoBehaviour
{
    // 有効か示すトリガー
    public bool isActive = false;


    /// <summary>
    /// 生成・再利用時の初期化処理
    /// </summary>
    /// <param name="pos"></param>
    public void Init(Vector3 pos)
    {
        transform.position = pos;

        isActive = true;
    }



    public void Disable()
    {
        isActive = false;
    }
}
