using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    // 生成する弾のPrefab
    [SerializeField] GameObject[] bulletPrefabs;
    [SerializeField] GameObject shockWavePrefab;

    // 弾の親オブジェクト
    [SerializeField] Transform BulletsParent;

    // 生成した弾を管理するリスト
    List<NormalBulletController> normals = new List<NormalBulletController>();
    List<ExplosionBulletController> explosions = new List<ExplosionBulletController>();
    List<ShockWaveController> shockWaves = new List<ShockWaveController>();


    /// <summary>
    /// 通常弾を作る関数
    /// </summary>
    /// <param name="barrel"></param>
    /// <param name="target"></param>
    public void CreateNormalBullet(GameObject target, Vector3 barrel)
    {
        // 無効状態の弾があれば再利用する
        foreach (NormalBulletController n in normals)
        {
            if (!n.isActive)
            {
                n.Init(target, barrel);
                return;
            }
        }

        // 無効状態の弾が無ければ新規作成してリストに加える
        NormalBulletController nb = Instantiate(bulletPrefabs[0], barrel, Quaternion.identity, BulletsParent).GetComponent<NormalBulletController>();
        nb.Init(target, barrel);
        normals.Add(nb);

    }


    /// <summary>
    /// 爆発弾を作る関数
    /// </summary>
    /// <param name="target"></param>
    /// <param name="barrel"></param>
    public void CreateExplosionBullet(GameObject target, Vector3 barrel)
    {
        // 無効状態の弾があれば再利用する
        foreach (ExplosionBulletController e in explosions)
        {
            if (!e.isActive)
            {
                e.Init(target, barrel);
                return;
            }
        }

        // 無効状態の弾が無ければ新規作成してリストに加える
        ExplosionBulletController eb = Instantiate(bulletPrefabs[1], barrel, Quaternion.identity, BulletsParent).GetComponent<ExplosionBulletController>();
        eb.Init(target, barrel);
        explosions.Add(eb);
    }


    /// <summary>
    /// 衝撃波を作る関数
    /// </summary>
    /// <param name="basePos"></param>
    public void CreateShockWave(Vector3 basePos)
    {
        // 無効状態の衝撃派があれば再利用する
        foreach (ShockWaveController s in shockWaves)
        {
            if (!s.isActive)
            {
                s.Init(basePos);
                return;
            }
        }

        ShockWaveController sw = Instantiate(shockWavePrefab, basePos, Quaternion.identity, BulletsParent).GetComponent<ShockWaveController>();
        sw.Init(basePos);
        shockWaves.Add(sw);
    }
}
