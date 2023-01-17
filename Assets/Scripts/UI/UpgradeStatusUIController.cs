using UnityEngine;
using UnityEngine.UI;

public class UpgradeStatusUIController : MonoBehaviour
{
    [SerializeField] Sprite activeImage;
    [SerializeField] Sprite inActiveImage;

    [SerializeField] Image[] upgradeParamImages1;
    [SerializeField] Image[] upgradeParamImages2;
    [SerializeField] Image[] upgradeParamImages3;

    int upgradeMax = 4;

    TowerController targetTC;
    int targetFloorNum;

    struct StatusData
    {
        public int[] param1;
        public int[] param2;
        public int[] param3;
    }

    StatusData statusData;

    [SerializeField] int[] param1;
    [SerializeField] int[] param2;


    void Awake()
    {
        statusData.param1 = param1;
        statusData.param2 = param2;
        statusData.param3 = null;
    }


    public void ShowUpgradeStatus(TowerController tc, int floorNum, int towerLevel)
    {
        targetTC = tc;
        targetFloorNum = floorNum;

        towerLevel -= 1;

        // iは画像のindex
        for (int i = 0; i < upgradeParamImages1.Length; i++)
        {
            // 関係ないゲージのアニメーション残りの削除
            upgradeParamImages1[i].GetComponent<Animator>().SetBool("isBlink", false);

            if (i < statusData.param1[towerLevel])
            {
                upgradeParamImages1[i].sprite = activeImage;
            }
            else if (towerLevel < upgradeMax && i < statusData.param1[towerLevel + 1])
            {
                // 能力が増える分はここ
                upgradeParamImages1[i].sprite = activeImage;
                upgradeParamImages1[i].GetComponent<Animator>().SetBool("isBlink", true);
            }
            else
            {
                upgradeParamImages1[i].sprite = inActiveImage;
            }
        }

        for (int i = 0; i < upgradeParamImages2.Length; i++)
        {
            // 関係ないゲージのアニメーション残りの削除
            upgradeParamImages2[i].GetComponent<Animator>().SetBool("isBlink", false);

            if (i < statusData.param2[towerLevel])
            {
                upgradeParamImages2[i].sprite = activeImage;
            }
            else if (towerLevel < upgradeMax && i < statusData.param2[towerLevel + 1])
            {
                // 能力が増える分はここ
                upgradeParamImages2[i].sprite = activeImage;
                upgradeParamImages2[i].GetComponent<Animator>().SetBool("isBlink", true);
            }
            else
            {
                upgradeParamImages2[i].sprite = inActiveImage;
            }
        }
    }


    public void OnUpgradeClick()
    {
        switch (targetFloorNum)
        {
            case 0:
                targetTC.UpgradeBotFloor();
                break;
            case 1:
                targetTC.UpgradeMidFloor();
                break;
            case 2:
                targetTC.UpgradeTopFloor();
                break;
        }

        UIManager.Instance.InvisibleUI();
    }
}
