using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Bullet;

public class TowerFloorController : MonoBehaviour
{
    // 発射する弾の種類
    [FormerlySerializedAs("bulletType")]
    [SerializeField] public BulletManager.BulletType bulletType_toChangeShot;

    [SerializeField] Transform muzzleTransform_toSetFirePos;

    // アウトライン
    [SerializeField] GameObject outlineObject;

    [SerializeField] TowerRangeController myRangeController;
    [SerializeField] Vector3 rangeScale = new Vector3(4, 1, 4);

    // 攻撃範囲内にいる敵のリスト
    List<GameObject> targetEnemyList_toFire = new List<GameObject>();

    // アップグレードを行った回数
    [System.NonSerialized] public int towerLevel_toUpgradeShots = 1;


    float baseAttackDamage = 5;

    float multiplyAttackDamage = 1f;

    float explosionRange_toMultiply = 1f;

    // 有効かを示すフラグ
    public bool isActive_toActivateUpdate = true;

    // 射撃してから経過した時間
    float timeFromLastShot_toCompareReloadTime = 0f;

    // 武装のリロードにかかる時間
    float reloadTime_toCompareTimeFromLastShot = 1f;

    // フィールドを出したか示すフラグ
    SlowFieldController mySlowField;


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
                BulletManager.Instance.CreateNormalBullet(this, baseAttackDamage * multiplyAttackDamage);
                break;

            case BulletManager.BulletType.ExplosionBullet:
                BulletManager.Instance.CreateExplosionBullet(this, baseAttackDamage * multiplyAttackDamage);
                break;

            case BulletManager.BulletType.PiercingBullet:
                BulletManager.Instance.CreatePiercingBullet(this, baseAttackDamage * multiplyAttackDamage);
                break;

            case BulletManager.BulletType.ShockWave:
                BulletManager.Instance.CreateShockWave(this, baseAttackDamage * multiplyAttackDamage);
                break;

            case BulletManager.BulletType.SlowField:
                if (mySlowField == null)
                {
                    mySlowField = BulletManager.Instance.CreateSlowField(this, baseAttackDamage * multiplyAttackDamage);
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
        towerLevel_toUpgradeShots += 1;

        switch (bulletType_toChangeShot)
        {
            case BulletManager.BulletType.NormalBullet:
                switch (towerLevel_toUpgradeShots)
                {
                    case 2:
                        multiplyAttackDamage = 1.25f;
                        break;
                    case 3:
                        multiplyAttackDamage = 2.0f;
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.25f;
                        break;
                    case 4:
                        multiplyAttackDamage = 2.5f;
                        break;
                    case 5:
                        multiplyAttackDamage = 3f;
                        myRangeController.gameObject.transform.localScale = rangeScale * 2f;
                        break;
                }
                break;

            case BulletManager.BulletType.ExplosionBullet:
                switch (towerLevel_toUpgradeShots)
                {
                    case 2:
                        multiplyAttackDamage = 1.25f;
                        break;
                    case 3:
                        multiplyAttackDamage = 1.5f;
                        explosionRange_toMultiply = 1.5f;
                        break;
                    case 4:
                        explosionRange_toMultiply = 1.75f;
                        break;
                    case 5:
                        multiplyAttackDamage = 2.0f;
                        explosionRange_toMultiply = 2.0f;
                        break;
                }
                break;

            case BulletManager.BulletType.PiercingBullet:
                switch (towerLevel_toUpgradeShots)
                {
                    case 2:
                        multiplyAttackDamage = 1.25f;
                        break;
                    case 3:
                        multiplyAttackDamage = 1.5f;
                        // 弾速1.25x
                        break;
                    case 4:
                        multiplyAttackDamage = 2.0f;
                        // 弾速1.5x
                        break;
                    case 5:
                        multiplyAttackDamage = 3.0f;
                        // 弾速2.0x
                        break;
                }
                break;

            case BulletManager.BulletType.ShockWave:
                switch (towerLevel_toUpgradeShots)
                {
                    case 2:
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.1f;
                        break;
                    case 3:
                        multiplyAttackDamage = 1.25f;
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.25f;
                        break;
                    case 4:
                        multiplyAttackDamage = 1.5f;
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.35f;
                        break;
                    case 5:
                        multiplyAttackDamage = 2.0f;
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.5f;
                        break;
                }
                break;

            case BulletManager.BulletType.SlowField:
                switch (towerLevel_toUpgradeShots)
                {
                    case 2:
                        mySlowField.ChangeSlowRatio(30);
                        break;
                    case 3:
                        mySlowField.ChangeSlowRatio(40);
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.25f;
                        break;
                    case 4:
                        mySlowField.ChangeSlowRatio(50);
                        break;
                    case 5:
                        mySlowField.ChangeSlowRatio(50);
                        myRangeController.gameObject.transform.localScale = rangeScale * 1.5f;
                        break;
                }
                break;
        }







        // TODO. アップグレード内容を考える
        reloadTime_toCompareTimeFromLastShot -= 0.05f;

        transform.parent.GetComponent<TowerController>().SetAllFloorsIsActive(true);

        UIManager.Instance.UpdateInfo();
    }
}