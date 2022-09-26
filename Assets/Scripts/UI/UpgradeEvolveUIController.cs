using UnityEngine;

public class UpgradeEvolveUIController : MonoBehaviour
{
    [SerializeField] TowerFloorController.BulletType[] bulletTypes;

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
    }



    public void OnConfirmButtonClick()
    {
        if (selectedIndex == -1)
            return;

        targetTC.ChangeFloor(targetFloorNum, bulletTypes[selectedIndex]);
    }

}
