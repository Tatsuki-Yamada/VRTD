﻿using System.Collections.Generic;
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


    /// <summary>
    /// 指定したフロアのアップグレードを開始する
    /// </summary>
    /// <param name="floorNum"></param>
    public void UpgradeFloor(int floorNum)
    {
        tfc[floorNum - 1].StartBuild();
        ConstructionManager.Instance.CreateConstructionSite(transform.position, tfc[floorNum - 1].bcc, 10, true);
    }


    /// <summary>
    /// 全フロアのisActiveを変更する関数
    /// </summary>
    /// <param name="b"></param>
    public void SetAllFloorsActive(bool b)
    {
        foreach (TowerFloorController tf in tfc)
        {
            tf.isActive = b;
        }
    }


    /// <summary>
    /// 全フロアのoutlineを変更する関数
    /// </summary>
    /// <param name="b"></param>
    public void SetAllFloorsOutline(bool b)
    {
        foreach (TowerFloorController tf in tfc)
        {
            tf.outline = b;
        }
    }


    /// <summary>
    /// 全フロアのTowerFloorControllerを返す関数
    /// </summary>
    /// <returns></returns>
    public TowerFloorController[] GetTFCs()
    {
        return tfc;
    }


    /// <summary>
    /// 全フロアのアップグレード回数を配列で返す関数
    /// </summary>
    /// <returns></returns>
    public int[] GetAllFloorsUpgradeCount()
    {
        int[] r = { tfc[0].upgradeCount, tfc[1].upgradeCount, tfc[2].upgradeCount };
        return r;
    }


    /// <summary>
    /// タワーが選択されたときに呼ばれる関数
    /// </summary>
    public void OnSelected()
    {
        UIManager.Instance.SetTowerController(this);
    }  
}
