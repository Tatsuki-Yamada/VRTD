using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    TowerFloorController[] tfc = new TowerFloorController[3];

    void Awake()
    {
        tfc[0] = transform.Find("TowerFloor_1").GetComponent<TowerFloorController>();
        tfc[1] = transform.Find("TowerFloor_2").GetComponent<TowerFloorController>();
        tfc[2] = transform.Find("TowerFloor_3").GetComponent<TowerFloorController>();
    }


    public TowerFloorController[] GetTFCs()
    {
        return tfc;
    }


    public void OnSelected()
    {
        UIManager.Instance.ActiveUI(gameObject);
    }

}
