using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class TowerFloorController : MonoBehaviour
{
    // 発射する弾の種類
    [FormerlySerializedAs("bulletType")]
    [SerializeField] public BulletManager.BulletType bulletType_toChangeShot;

    [SerializeField] Transform muzzleTransform_toSetFirePos;

    // アウトライン
    [SerializeField] GameObject outlineObject;

    // 攻撃範囲内にいる敵のリスト
    List<GameObject> targetEnemyList_toFire = new List<GameObject>();

    // アップグレードを行った回数
    [System.NonSerialized] public int towerLevel_toUpgradeShots = 1;

    // 有効かを示すフラグ
    public bool isActive_toActivateUpdate = true;

    // 射撃してから経過した時間
    float timeFromLastShot_toCompareReloadTime = 0f;

    // 武装のリロードにかかる時間
    float reloadTime_toCompareTimeFromLastShot = 1f;

    // フィールドを出したか示すフラグ
    bool isActivatedField = false;


    /// <summary>
    /// アウトラインの表示・非表示を設定するプロパティ
    /// </summary>
    public bool outline
    {
        get
        {
            return outline;
        }

        set
        {
            outlineObject.SetActive(value);
        }
    }


    private void Update()
    {
        if (!isActive_toActivateUpdate)
            return;

        RotateToEnemy();
        Shot();
    }


    public void AddEnemyToTargetList(GameObject addEnemy)
    {
        targetEnemyList_toFire.Add(addEnemy);
    }


    public void RemoveEnemyFromTargetList(GameObject removeEnemy)
    {
        int indexNum = targetEnemyList_toFire.IndexOf(removeEnemy);

        if (indexNum == -1)
        {
            Debug.LogError("This enemy is not contained in targetEnemyList.");
            return;
        }
        targetEnemyList_toFire.RemoveAt(indexNum);
    }


    private void RotateToEnemy()
    {
        if (targetEnemyList_toFire.Count == 0)
            return;

        transform.LookAt(targetEnemyList_toFire[0].transform.position);
        // X軸方向の角度を0にして、上下を向かないようにする。
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z));
    }


    private void Shot()
    {
        timeFromLastShot_toCompareReloadTime += Time.deltaTime;

        if (targetEnemyList_toFire.Count == 0)
            return;

        if (timeFromLastShot_toCompareReloadTime < reloadTime_toCompareTimeFromLastShot)
            return;

        switch (bulletType_toChangeShot)
        {
            case BulletManager.BulletType.NormalBullet:
                BulletManager.Instance.CreateNormalBullet(this);
                break;

            case BulletManager.BulletType.ExplosionBullet:
                BulletManager.Instance.CreateExplosionBullet(this);
                break;

            case BulletManager.BulletType.PiercingBullet:
                BulletManager.Instance.CreatePiercingBullet(this);
                break;

            case BulletManager.BulletType.ShockWave:
                BulletManager.Instance.CreateShockWave(this);
                break;

            case BulletManager.BulletType.SlowField:
                if (isActivatedField == false)
                {
                    BulletManager.Instance.CreateSlowField(this);
                    isActivatedField = true;
                }
                break;

        }

        timeFromLastShot_toCompareReloadTime = 0f;
    }


    public GameObject GetFirstTargetableEnemy()
    {
        if (targetEnemyList_toFire.Count == 0)
            Debug.LogError("no objects in targetEnemyList.");

        return targetEnemyList_toFire[0];
    }


    public Vector3 GetMuzzlePosition()
    {
        return muzzleTransform_toSetFirePos.position;
    }


    public void StartUpgrade()
    {
        if (isActive_toActivateUpdate == false)
            return;

        ConstructionSiteController tempCsc_toCallSometime = ConstructionManager.Instance.CreateContructionTowerSite(transform.parent.position, 10 + towerLevel_toUpgradeShots * 2);
        tempCsc_toCallSometime.onCompleteBuildFuncs_toCallback.AddListener(CompleteUpgrade);

        transform.parent.GetComponent<TowerController>().SetAllFloorsIsActive(false);
    }


    /// <summary>
    /// アップグレードが終わったとき、コールバックされる関数
    /// </summary>
    public void CompleteUpgrade()
    {
        // TODO. アップグレード内容を考える
        reloadTime_toCompareTimeFromLastShot -= 0.1f;
        towerLevel_toUpgradeShots += 1;

        transform.parent.GetComponent<TowerController>().SetAllFloorsIsActive(true);

        UIManager.Instance.UpdateInfo();
    }
}