using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Bullet;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    public enum BulletType
    {
        NormalBullet = 0,
        ExplosionBullet = 1,
        PiercingBullet = 2,
        ShockWave = 3,
        SlowField = 4,
    }

    // 生成する弾のPrefabリスト
    [SerializeField] GameObject[] bulletPrefabList_toInstantiate;

    // 生成した弾をまとめる親オブジェクト
    [SerializeField] Transform bulletsParent_toGroup;

    // 生成した弾をまとめるリスト
    List<Object> bulletList_toReuse = new List<Object>();


    public void CreateNormalBullet(TowerFloorController tfc_toGetEnemyAndBarrelData)
    {
        CreateBullet<NormalBulletController>(tfc_toGetEnemyAndBarrelData);
    }

    public void CreateExplosionBullet(TowerFloorController tfc_toGetEnemyAndBarrelData)
    {
        CreateBullet<ExplosionBulletController>(tfc_toGetEnemyAndBarrelData);
    }

    public void CreatePiercingBullet(TowerFloorController tfc_toGetEnemyAndBarrelData)
    {
        CreateBullet<PiercingBulletController>(tfc_toGetEnemyAndBarrelData);
    }

    public void CreateShockWave(TowerFloorController tfc_toGetEnemyAndBarrelData)
    {
        CreateBullet<ShockWaveController>(tfc_toGetEnemyAndBarrelData);
    }

    public void CreateSlowField(TowerFloorController tfc_toGetEnemyAndBarrelData)
    {
        CreateBullet<SlowFieldController>(tfc_toGetEnemyAndBarrelData);
    }


    /// <summary>
    /// 一般化した、弾を生成するメソッド。
    /// </summary>
    /// <param name="tfc_toGetEnemyAndBarrelData"></param>
    /// <typeparam name="T"></typeparam>
    void CreateBullet<T>(TowerFloorController tfc_toGetEnemyAndBarrelData) where T : BulletController
    {
        // bulletListからTだけのリストを作成する。
        List<T> oneTypeList_toSearchDisableBullet = bulletList_toReuse.OfType<T>().ToList();

        // 無効状態の弾があれば再利用する。
        foreach (T type in oneTypeList_toSearchDisableBullet)
        {
            if (type.isActive_toActivateUpdate == false)
            {
                type.Init(tfc_toGetEnemyAndBarrelData);
                return;
            }
        }

        GameObject toInstantiateObj = null;

        // TをアタッチしたPrefabを探す。
        foreach (GameObject obj_toSearchHasTypeComponent in bulletPrefabList_toInstantiate)
        {
            if (obj_toSearchHasTypeComponent.TryGetComponent<T>(out T temp))
            {
                toInstantiateObj = obj_toSearchHasTypeComponent;
                break;
            }
        }

        // TをアタッチしたPrefabが見つからなかったらエラー
        if (toInstantiateObj == null)
            Debug.LogError("BulletManager: toInstantiateObj = null");

        T typeObj_toAddList = Instantiate(toInstantiateObj, parent: bulletsParent_toGroup).GetComponent<T>();
        typeObj_toAddList.Init(tfc_toGetEnemyAndBarrelData);
        bulletList_toReuse.Add(typeObj_toAddList);
    }

}
