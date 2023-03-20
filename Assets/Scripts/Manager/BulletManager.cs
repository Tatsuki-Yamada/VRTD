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


    public void CreateNormalBullet(TowerFloorController tfc_toGetEnemyAndBarrelData, float bulletDamage = 5f)
    {
        CreateBullet<NormalBulletController>(tfc_toGetEnemyAndBarrelData, bulletDamage);
    }

    public ExplosionBulletController CreateExplosionBullet(TowerFloorController tfc_toGetEnemyAndBarrelData, float bulletDamage = 5f)
    {
        return CreateBullet<ExplosionBulletController>(tfc_toGetEnemyAndBarrelData, bulletDamage);
    }

    public PiercingBulletController CreatePiercingBullet(TowerFloorController tfc_toGetEnemyAndBarrelData, float bulletDamage = 5f)
    {
        return CreateBullet<PiercingBulletController>(tfc_toGetEnemyAndBarrelData, bulletDamage);
    }

    public void CreateShockWave(TowerFloorController tfc_toGetEnemyAndBarrelData, float bulletDamage = 5f)
    {
        CreateBullet<ShockWaveController>(tfc_toGetEnemyAndBarrelData, bulletDamage);
    }

    public SlowFieldController CreateSlowField(TowerFloorController tfc_toGetEnemyAndBarrelData, float bulletDamage = 5f)
    {
        return CreateBullet<SlowFieldController>(tfc_toGetEnemyAndBarrelData, bulletDamage);
    }


    /// <summary>
    /// 一般化した、弾を生成するメソッド。
    /// </summary>
    /// <param name="tfc_toGetEnemyAndBarrelData"></param>
    /// <typeparam name="T"></typeparam>
    private T CreateBullet<T>(TowerFloorController tfc_toGetEnemyAndBarrelData, float bulletDamage = 5f) where T : BulletController
    {
        // bulletListからTだけのリストを作成する。
        List<T> oneTypeList_toSearchDisableBullet = bulletList_toReuse.OfType<T>().ToList();

        // 無効状態の弾があれば再利用する。
        foreach (T type in oneTypeList_toSearchDisableBullet)
        {
            if (type.isActive_toActivateUpdate == false)
            {
                type.Init(tfc_toGetEnemyAndBarrelData, (int)bulletDamage);
                return type;
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
        typeObj_toAddList.Init(tfc_toGetEnemyAndBarrelData, (int)bulletDamage);
        bulletList_toReuse.Add(typeObj_toAddList);
        return typeObj_toAddList;
    }
}
