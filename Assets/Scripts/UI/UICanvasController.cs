using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICanvasController : MonoBehaviour
{
    [SerializeField] Camera targetCamera;

    TowerController targetTC;

    [SerializeField] Button[] upgradeButtons;


    void Update()
    {
        transform.LookAt(targetCamera.transform);
        transform.Rotate(0, 180, 0);
    }


    /// <summary>
    /// 対象のTowerControllerを取得する関数
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetTower(TowerController tc)
    {
        targetTC = tc;

        UpdateInfo();
    }


    /// <summary>
    /// ボタンの表示などを更新する関数
    /// </summary>
    public void UpdateInfo()
    {
        int[] upgradeCounts = targetTC.GetAllFloorsUpgradeCount();

        for (int i = 0; i < upgradeCounts.Length; i++)
        {
            if (upgradeCounts[i] >= 5)
            {
                upgradeButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "進化";
            }
            else
            {
                upgradeButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "アップグレード";
            }
        }
    }

    /// <summary>
    /// アップグレードボタンが押されたときに呼ばれる関数
    /// </summary>
    /// <param name="floorNum"></param>
    public void OnUpgradeClick(int floorNum)
    {
        targetTC.UpgradeFloor(floorNum);
    }
}
