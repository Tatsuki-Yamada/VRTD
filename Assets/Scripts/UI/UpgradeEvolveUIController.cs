using UnityEngine;

public class UpgradeEvolveUIController : MonoBehaviour
{
    [SerializeField] BulletManager.BulletType[] bulletTypes;

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

        switch (targetFloorNum)
        {
            case 0:
                targetTC.ChangeBotFloor(bulletTypes[selectedIndex]);
                break;
            case 1:
                targetTC.ChangeMidFloor(bulletTypes[selectedIndex]);
                break;
            case 2:
                targetTC.ChangeTopFloor(bulletTypes[selectedIndex]);
                break;
        }

        UIManager.Instance.InvisibleUI();
    }

}
