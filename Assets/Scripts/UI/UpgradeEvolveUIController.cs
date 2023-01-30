using UnityEngine;
using UnityEngine.UI;

public class UpgradeEvolveUIController : MonoBehaviour
{
    [SerializeField] BulletManager.BulletType[] bulletTypes;
    [SerializeField] Toggle[] toggles;

    int selectedIndex = -1;

    TowerController targetTC;
    int targetFloorNum;


    public void ShowEvovleUI(TowerController tc, int floorNum)
    {
        targetTC = tc;
        targetFloorNum = floorNum;
    }


    public void OnSelectButtonClick(int selectIndex)
    {
        selectedIndex = selectIndex;

        foreach (Toggle t in toggles)
        {
            t.isOn = false;
        }

        toggles[selectedIndex].isOn = true;
    }



    public void OnConfirmButtonClick()
    {
        if (selectedIndex == -1)
            return;

        ConstructionSiteController tempCsc_toCallSometime = ConstructionManager.Instance.CreateContructionTowerSite(targetTC.transform.position, 30);
        tempCsc_toCallSometime.targetTC = targetTC;
        tempCsc_toCallSometime.toChangeType = bulletTypes[selectedIndex];
        tempCsc_toCallSometime.floorNum = targetFloorNum;

        targetTC.SetAllFloorsIsActive(false);

        tempCsc_toCallSometime.onCompleteBuildFuncs_toCallback.AddListener(tempCsc_toCallSometime.Evolve);


        UIManager.Instance.InvisibleUI();
    }

}
