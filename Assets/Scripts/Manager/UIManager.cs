using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] Camera centerEyeCamera;

    [SerializeField] GameObject HandyUICnavas;

    TowerController selectedTowerController;
    



    public void SetTowerController(TowerController tower)
    {
        ResetHighlights();

        selectedTowerController = tower;
        selectedTowerController.SetAllFloorsOutline(true);

        HandyUICnavas.SetActive(true);
        HandyUICnavas.GetComponent<UICanvasController>().SetTargetTower(tower);

    }


    /// <summary>
    /// オブジェクトのハイライトを消す関数
    /// </summary>
    void ResetHighlights()
    {
        if (selectedTowerController != null)
            selectedTowerController.SetAllFloorsOutline(false);
    }
}
