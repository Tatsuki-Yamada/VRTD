using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // 攻撃範囲を管理する子スクリプト
    TowerRangeController rangeController;

    // 敵が攻撃範囲内にいるか示す変数
    bool enemiesInRange = false;

    // 攻撃範囲内にいる敵のリスト
    List<GameObject> targetEnemyList = new List<GameObject>();

    // 有効になるまでのパンチ回数
    int leftToActivate = 20;

    // 武装のリロードにかかる時間
    float reloadTime = 1f;

    // 射撃してから経過した時間
    float timeFromLastShot = 0f;

    // 発射する弾のPrefab
    [SerializeField] GameObject bulletPrefab;


    void Awake()
    {
        rangeController = transform.GetChild(0).GetComponent<TowerRangeController>();
    }


    void FixedUpdate()
    {
        Attack();
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


    // 攻撃する処理
    void Attack()
    {
        timeFromLastShot += Time.deltaTime;

        if (enemiesInRange)
        {
            if (timeFromLastShot > reloadTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
                bullet.GetComponent<BulletController>().SetTarget(targetEnemyList[0]);
                timeFromLastShot = 0f;
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            leftToActivate--;
        }
    }
}
