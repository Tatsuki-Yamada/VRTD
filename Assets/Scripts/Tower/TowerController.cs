using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    TowerFloorController[] myTowerFloorControllers = new TowerFloorController[3];

    [FormerlySerializedAs("towerFloorPrefabs")]
    [SerializeField] GameObject[] towerFloorPrefabs_toChangeShot;


    private void Awake()
    {
        myTowerFloorControllers[0] = transform.Find("TowerFloor_1").GetComponent<TowerFloorController>();
        myTowerFloorControllers[1] = transform.Find("TowerFloor_2").GetComponent<TowerFloorController>();
        myTowerFloorControllers[2] = transform.Find("TowerFloor_3").GetComponent<TowerFloorController>();
    }


    public void UpgradeTopFloor()
    {
        UpgradeFloor(2);
    }

    public void UpgradeMidFloor()
    {
        UpgradeFloor(1);
    }

    public void UpgradeBotFloor()
    {
        UpgradeFloor(0);
    }

    private void UpgradeFloor(int floorNum_toUpgradeThisIndexFloor)
    {
        myTowerFloorControllers[floorNum_toUpgradeThisIndexFloor].StartUpgrade();
    }


    public void ChangeTopFloor(BulletManager.BulletType bulletType_toChange)
    {
        ChangeFloor(2, bulletType_toChange);
    }

    public void ChangeMidFloor(BulletManager.BulletType bulletType_toChange)
    {
        ChangeFloor(1, bulletType_toChange);
    }

    public void ChangeBotFloor(BulletManager.BulletType bulletType_toChange)
    {
        ChangeFloor(0, bulletType_toChange);
    }

    private void ChangeFloor(int floorNum_toChangeThisIndexTower, BulletManager.BulletType bulletType)
    {
        GameObject newFloor = Instantiate(towerFloorPrefabs_toChangeShot[(int)bulletType], myTowerFloorControllers[floorNum_toChangeThisIndexTower].transform.position, Quaternion.identity, transform);
        newFloor.name = "TowerFloor_" + (floorNum_toChangeThisIndexTower + 1).ToString();
        Destroy(myTowerFloorControllers[floorNum_toChangeThisIndexTower].gameObject);
        myTowerFloorControllers[floorNum_toChangeThisIndexTower] = newFloor.GetComponent<TowerFloorController>();
    }


    public void SetAllFloorsIsActive(bool flag_toSetThis)
    {
        foreach (TowerFloorController tf in myTowerFloorControllers)
        {
            tf.isActive_toActivateUpdate = flag_toSetThis;
        }
    }


    public void SetAllFloorsOutline(bool flag_toSetThis)
    {
        return;
        foreach (TowerFloorController tf in myTowerFloorControllers)
        {
            tf.outline = flag_toSetThis;
        }
    }

    public TowerFloorController[] GetChiledTFCs()
    {
        return myTowerFloorControllers;
    }

    /// <summary>
    /// 全フロアのアップグレード回数を配列で返す関数
    /// </summary>
    /// <returns></returns>
    public int[] GetAllFloorsLevel()
    {
        int[] return_list = { myTowerFloorControllers[0].towerLevel_toUpgradeShots, myTowerFloorControllers[1].towerLevel_toUpgradeShots, myTowerFloorControllers[2].towerLevel_toUpgradeShots };
        return return_list;
    }


    /// <summary>
    /// タワーが選択されたときに呼ばれる関数
    /// </summary>
    public void OnSelectedThisTower()
    {
        UIManager.Instance.SetTowerController(this);
    }
}
