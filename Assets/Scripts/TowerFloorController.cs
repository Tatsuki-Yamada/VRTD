using UnityEngine;
using System.Collections.Generic;

public class TowerFloorController : MonoBehaviour
{
    // 発射する弾のPrefab
    [SerializeField] GameObject bulletPrefab;

    // 発射する砲身のTransform
    [SerializeField] Transform barrelTransform;

    // 敵が攻撃範囲内にいるか示す変数
    bool enemiesInRange = false;

    // 射撃してから経過した時間
    float timeFromLastShot = 0f;

    // 武装のリロードにかかる時間
    float reloadTime = 1f;

    // 攻撃範囲内にいる敵のリスト
    List<GameObject> targetEnemyList = new List<GameObject>();

    // 攻撃範囲を管理する子スクリプト
    TowerRangeController rangeController;


    void Awake()
    {
        rangeController = transform.GetChild(0).GetComponent<TowerRangeController>();
    }


    void Update()
    {
        if (enemiesInRange)
        {
            if (!(targetEnemyList[0].transform.position == new Vector3(50, 50, 50)))
            {
                transform.LookAt(targetEnemyList[0].transform.position);
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z));
            }
        }

        Shoot();
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

    void Shoot()
    {
        
        timeFromLastShot += Time.deltaTime;

        if (enemiesInRange)
        {
            if (timeFromLastShot > reloadTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, transform.rotation);
                bullet.GetComponent<BulletController>().SetTarget(targetEnemyList[0]);
                timeFromLastShot = 0f;
            }
        }
        
    }
}
