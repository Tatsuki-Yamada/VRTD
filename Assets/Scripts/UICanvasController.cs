using UnityEngine;

public class UICanvasController : MonoBehaviour
{
    [SerializeField] Camera targetCamera;

    TowerController targetTC;


    void Update()
    {
        transform.LookAt(targetCamera.transform);
        transform.Rotate(0, 180, 0);
    }


    /// <summary>
    /// 対象のTowerControllerを取得する関数
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetTower(GameObject target)
    {
        targetTC = target.GetComponent<TowerController>();
    }


    /// <summary>
    /// アップグレードボタンが押されたときに呼ばれる関数
    /// </summary>
    /// <param name="floorNum"></param>
    public void OnUpgradeClick(int floorNum)
    {
        targetTC.GetTFCs()[floorNum].StartBuild();
    }
}
