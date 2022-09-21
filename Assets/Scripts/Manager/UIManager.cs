using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] Camera centerEyeCamera;

    [SerializeField] UICanvasController handyUICnavas;

    TowerController selectedTowerController;
    



    public void SetTowerController(TowerController tower)
    {
        ResetHighlights();

        selectedTowerController = tower;
        selectedTowerController.SetAllFloorsOutline(true);

        handyUICnavas.gameObject.SetActive(true);
        handyUICnavas.GetComponent<UICanvasController>().SetTargetTower(tower);
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
        handyUICnavas.UpdateInfo();
    }
}
