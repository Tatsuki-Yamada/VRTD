﻿using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    // 生成する弾のPrefab
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shockWavePrefab;

    // 弾の親オブジェクト
    [SerializeField] Transform BulletsParent;

    // 生成した弾を管理するリスト
    List<BulletController> bullets = new List<BulletController>();
    List<ShockWaveController> shockWaves = new List<ShockWaveController>();
    

    /// <summary>
    /// 弾を生成・再利用する関数
    /// </summary>
    /// <param name="barrel"></param>
    /// <param name="target"></param>
    public void CreateBullet(Vector3 barrel, GameObject target)
    {
        // 無効状態の弾があれば再利用する
        foreach (BulletController b in bullets)
        {
            if (!b.isActive)
            {
                b.transform.position = barrel;
                b.Reset(target);
                return;
            }
        }

        // 無効状態の弾が無ければ新規作成してリストに加える
        BulletController bc = Instantiate(bulletPrefab, barrel, Quaternion.identity, BulletsParent).GetComponent<BulletController>();
        bc.SetTarget(target);
        bullets.Add(bc);
    }


    public void CreateShockWave(Vector3 basePos)
    {
        // 無効状態の衝撃派があれば再利用する
        foreach (ShockWaveController s in shockWaves)
        {
            if (!s.isActive)
            {
                s.transform.position = basePos;
                s.Reset();
                return;
            }
        }

        ShockWaveController sc = Instantiate(shockWavePrefab, basePos, Quaternion.identity, BulletsParent).GetComponent<ShockWaveController>();
        sc.Reset();
        shockWaves.Add(sc);
    }
}
