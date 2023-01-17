using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // タワーのアップグレード関係のUI
    [SerializeField] UpgradeUIController towerUICanvas;
    [SerializeField] UpgradeStatusUIController[] towerStatusUICanvases;
    [SerializeField] UpgradeEvolveUIController[] towerEvolveUICanvas;

    // 選択しているタワーコントローラー
    TowerController selectedTowerController;

    [SerializeField] public TextMeshProUGUI waveCountText;
    [SerializeField] public TextMeshProUGUI nextWaveTimeText;





    /// <summary>
    /// タワーを選択したときに呼ばれる関数
    /// </summary>
    /// <param name="tower"></param>
    public void SetTowerController(TowerController tower)
    {
        ResetHighlights();

        selectedTowerController = tower;
        selectedTowerController.SetAllFloorsOutline(true);

        towerUICanvas.gameObject.SetActive(true);
        towerUICanvas.GetComponent<UpgradeUIController>().SetTargetTower(tower);
    }


    /// <summary>
    /// タワー以外を選択したときの処理をまとめた関数
    /// </summary>
    public void SelectOutsideOfTower()
    {
        ResetHighlights();
        InvisibleUI();
    }


    /// <summary>
    /// オブジェクトのハイライトを消す関数
    /// </summary>
    void ResetHighlights()
    {
        if (selectedTowerController != null)
            selectedTowerController.SetAllFloorsOutline(false);
    }


    /// <summary>
    /// UIに更新の指示を出す関数
    /// </summary>
    public void UpdateInfo()
    {
        towerUICanvas.UpdateInfo();
    }


    /// <summary>
    /// UIを非表示にする関数
    /// </summary>
    public void InvisibleUI()
    {
        towerUICanvas.gameObject.SetActive(false);

        foreach (UpgradeStatusUIController ui in towerStatusUICanvases)
        {
            ui.gameObject.SetActive(false);
        }

        foreach (UpgradeEvolveUIController ev in towerEvolveUICanvas)
        {
            ev.gameObject.SetActive(false);
        }
    }


    public void InvisibleUI_NotIncludeCanvas()
    {
        foreach (UpgradeStatusUIController ui in towerStatusUICanvases)
        {
            ui.gameObject.SetActive(false);
        }

        foreach (UpgradeEvolveUIController ev in towerEvolveUICanvas)
        {
            ev.gameObject.SetActive(false);
        }
    }


    public void SetWaveTimer(int time)
    {
        StartCoroutine(WaveTimer(time));
    }


    private IEnumerator WaveTimer(int time)
    {
        Debug.Log(time);
        while (time > 0)
        {
            nextWaveTimeText.text = $"次のウェーブ開始まで：{time}秒";
            time -= 1;
            yield return new WaitForSeconds(1);
        }
    }
}
