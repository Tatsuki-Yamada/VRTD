using UnityEngine;

/// <summary>
/// タワーの攻撃範囲への敵の入出を検知し、親に渡すスクリプト
/// </summary>
public class TowerRangeController : MonoBehaviour
{
    // 親のスクリプト
    TowerFloorController towerFloorController;

    // 親のタワーが有効になっているか
    public bool isTowerActive = false;


    void Awake()
    {
        towerFloorController = transform.parent.GetComponent<TowerFloorController>();
    }


    /// <summary>
    /// 敵が入ったときの処理
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (isTowerActive)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                towerFloorController.AddEnemyToTargetList(other.gameObject);
            }
        }
    }


    /// <summary>
    /// 敵が出たときの処理
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (isTowerActive)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                towerFloorController.RemoveEnemyFromTargetList(other.gameObject);
            }
        }
    }
}
