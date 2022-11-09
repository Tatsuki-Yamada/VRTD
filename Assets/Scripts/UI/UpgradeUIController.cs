using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIController : MonoBehaviour
{
    // アップグレード結果を示すCanvasのコントローラーのリスト
    [SerializeField] UpgradeStatusUIController[] upgradeStatusUIControllers;

    [SerializeField] UpgradeEvolveUIController[] upgradeEvolveUIControllers;

    // 選択したタワーのコントローラたち
    TowerController targetTC;
    TowerFloorController[] targetTFCs;

    // Canvasのアップグレードボタンのリスト
    [SerializeField] Button[] upgradeButtons;

    // CanvasのImageオブジェクトと変更先のカラーのリスト
    [SerializeField] Image[] floorImages;
    [SerializeField] Color[] floorColors;


    /// <summary>
    /// 対象のTowerControllerを取得する関数
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetTower(TowerController tc)
    {
        targetTC = tc;
        targetTFCs = targetTC.GetChiledTFCs();

        UpdateInfo();
    }


    /// <summary>
    /// ボタンの表示などを更新する関数
    /// </summary>
    public void UpdateInfo()
    {
        for (int i = 0; i < targetTFCs.Length; i++)
        {
            switch (targetTFCs[i].bulletType_toChangeShot)
            {
                case BulletManager.BulletType.NormalBullet:
                    floorImages[i].color = floorColors[0];
                    break;

                case BulletManager.BulletType.ExplosionBullet:
                    floorImages[i].color = floorColors[1];
                    break;

                case BulletManager.BulletType.PiercingBullet:
                    floorImages[i].color = floorColors[2];
                    break;

                case BulletManager.BulletType.ShockWave:
                    floorImages[i].color = floorColors[3];
                    break;

                case BulletManager.BulletType.SlowField:
                    floorImages[i].color = floorColors[4];
                    break;

            }
        }

        // アップグレード回数に応じた挙動の変更
        int[] towerLevels = targetTC.GetAllFloorsLevel();

        for (int i = 0; i < towerLevels.Length; i++)
        {
            if (towerLevels[i] >= 5)
            {
                upgradeButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "進化";
            }
            else
            {
                upgradeButtons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "アップグレード";
            }
        }

    }

    /// <summary>
    /// アップグレードボタンが押されたときに呼ばれる関数
    /// </summary>
    /// <param name="floorNum"></param>
    public void OnUpgradeClick(int floorNum)
    {
        int[] towerLevels = targetTC.GetAllFloorsLevel();

        if (towerLevels[floorNum] >= 6)
        {
            upgradeEvolveUIControllers[floorNum].gameObject.SetActive(true);
            upgradeEvolveUIControllers[floorNum].ShowEvovleUI(targetTC, floorNum);
        }
        else
        {
            upgradeStatusUIControllers[(int)targetTFCs[floorNum].bulletType_toChangeShot].gameObject.SetActive(true);
            upgradeStatusUIControllers[(int)targetTFCs[floorNum].bulletType_toChangeShot].ShowUpgradeStatus(targetTC, floorNum, targetTFCs[floorNum].towerLevel_toUpgradeShots);
        }
    }
}
