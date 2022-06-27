using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] Camera centerEyeCamera;

    [SerializeField] GameObject HandyUICnavas;


    Vector3 offset = new Vector3(0, 0, 1f);



    public void ActiveUI(GameObject tower)
    {
        HandyUICnavas.SetActive(true);
        HandyUICnavas.GetComponent<UICanvasController>().SetTargetTower(tower);

    }
}
