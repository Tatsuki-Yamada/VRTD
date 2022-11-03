using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class TowerFloorController : BuildableObject
{
    public enum BulletType
    {
        NormalBullet = 0,
        ExplosionBullet = 1,
        PiercingBullet = 2,
        ShockWave = 3,
        SlowField = 4,
    }


    // 発射する砲身のTransform
    [FormerlySerializedAs("barrelTransform")]
    [SerializeField] Transform muzzleTransform_toSetFirePos;

    // 建設段階と完成後のMaterial
    [SerializeField] Material[] towerFloorMaterials = new Material[2];

    // 発射する弾の種類
    public BulletType bulletType;

    // 敵が攻撃範囲内にいるか示す変数
    bool enemiesInRange = false;

    // 射撃してから経過した時間
    float timeFromLastShot = 0f;

    // 武装のリロードにかかる時間
    float reloadTime = 1f;

    // アップグレードを行った回数
    public int towerLevel = 1;

    // 攻撃範囲内にいる敵のリスト
    List<GameObject> targetEnemyList = new List<GameObject>();

    // 攻撃範囲を管理する子スクリプト
    TowerRangeController rangeController;

    // このフロアを管理しているスクリプト
    TowerController tc;

    // 有効かを示すフラグ
    public bool isActive { get; set; } = true;

    // アウトライン
    [SerializeField] GameObject outlineObject;

    // 出しているフィールド
    GameObject field;

    // フィールドを出したか示すフラグ
    bool isFieldActive = false;


    /// <summary>
    /// アウトラインの表示・非表示を設定する
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



    public override void Awake()
    {
        base.Awake();
        tc = transform.parent.GetComponent<TowerController>();
        //rangeController = transform.GetChild(0).GetComponent<TowerRangeController>();
    }


    void Update()
    {
        if (isBuilding)
            return;

        if (!isActive)
            return;


        RotateToEnemy();

        // 弾の種類で動作を変える
        if (bulletType == BulletType.SlowField)
        {
            GenerateField();
        }
        else
        {
            Shoot();
        }
    }


    // 攻撃可能リストに追加する処理
    public void SetTargetEnemy(GameObject enemy)
    {
        enemiesInRange = true;
        targetEnemyList.Add(enemy);
    }


    // 攻撃可能リストから除外する処理
    public void RemoveTargetEnemy(GameObject enemy)
    {
        int i = targetEnemyList.IndexOf(enemy);
        if (i != -1)
        {
            targetEnemyList.RemoveAt(i);
        }

        if (targetEnemyList.Count == 0)
        {
            enemiesInRange = false;
        }
    }


    /// <summary>
    /// 敵に砲身を向ける関数
    /// </summary>
    void RotateToEnemy()
    {
        if (enemiesInRange)
        {
            if (!(targetEnemyList[0].transform.position == new Vector3(50, 50, 50)))
            {
                transform.LookAt(targetEnemyList[0].transform.position);
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z));
            }
        }
    }



    /// <summary>
    /// リロード時間の管理と弾を発射するまでの処理を行う関数
    /// </summary>
    void Shoot()
    {
        timeFromLastShot += Time.deltaTime;

        if (enemiesInRange)
        {
            if (timeFromLastShot > reloadTime)
            {
                switch (bulletType)
                {
                    case BulletType.NormalBullet:
                        BulletManager.Instance.CreateNormalBullet(this);
                        break;

                    case BulletType.ExplosionBullet:
                        BulletManager.Instance.CreateExplosionBullet(this);
                        break;

                    case BulletType.PiercingBullet:
                        BulletManager.Instance.CreatePiercingBullet(this);
                        break;

                    case BulletType.ShockWave:
                        BulletManager.Instance.CreateShockWave(this);
                        break;

                }

                timeFromLastShot = 0f;
            }
        }
    }


    /// <summary>
    /// 1度しか発射しないフィールドを生成する
    /// </summary>
    void GenerateField()
    {
        if (!isFieldActive)
        {
            BulletManager.Instance.CreateSlowField(this);
            isFieldActive = true;
        }
    }


    public GameObject GetFirstTargetableEnemy()
    {
        return targetEnemyList[0];
    }


    public Vector3 GetMuzzlePosition()
    {
        return muzzleTransform_toSetFirePos.position;
    }


    /// <summary>
    /// 改造を始める関数
    /// </summary>
    public override void StartBuild()
    {
        if (isBuilding || !isActive)
            return;

        base.StartBuild();

        // transform.GetChild(0).GetComponent<Renderer>().material = towerFloorMaterials[0];
        bcc.SetCount(10);
        bcc.onCompleteBuild.AddListener(CompleteBuild);

        tc.SetAllFloorsActive(false);
    }


    /// <summary>
    /// 改造が終わったとき、コールバックされる関数
    /// </summary>
    public override void CompleteBuild()
    {
        base.CompleteBuild();

        // transform.GetChild(0).GetComponent<Renderer>().material = towerFloorMaterials[1];

        // 試験用のアップグレード内容
        reloadTime -= 0.1f;

        tc.SetAllFloorsActive(true);

        bcc.onCompleteBuild.RemoveAllListeners();

        UIManager.Instance.UpdateInfo();

        towerLevel += 1;
    }
}