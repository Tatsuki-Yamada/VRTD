using UnityEngine;

/// <summary>
/// 敵に向かって飛んでいくタイプの弾の基底クラス
/// </summary>
public class BulletController : MonoBehaviour
{
    // ターゲットのTransform
    Transform target;

    // 自分のRigidbody
    Rigidbody rig;

    // 自分のCollider
    Collider col;

    // 敵に与えるダメージ
    [SerializeField] protected int damage = 5;

    // 移動スピード
    [SerializeField] float moveSpeed = 3f;

    // 有効かを示すフラグ
    public bool isActive = true;

    // 敵を貫通するか
    bool isThroughEnemy = false;


    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }


    /// <summary>
    /// 生成・再利用時の初期化処理を行う関数
    /// </summary>
    /// <param name="tgt"></param>
    public void Init(GameObject tgt, Vector3 pos)
    {
        target = tgt.transform;
        transform.position = pos;

        col.enabled = true;
        rig.isKinematic = false;

        isActive = true;
    }


    void FixedUpdate()
    {
        if (isActive)
        {
            Move();

            if (target.position == new Vector3(50, 50, 50))
            {
                Delete();
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Hit(other);

            if (!isThroughEnemy)
            {
                Delete();
            }
        }
    }


    /// <summary>
    /// 目標に向かって移動する関数
    /// </summary>
    void Move()
    {
        transform.LookAt(target.position);
        rig.AddRelativeForce(Vector3.forward * moveSpeed);
    }


    /// <summary>
    /// 当たった敵にダメージを与える関数
    /// </summary>
    /// <param name="hitEnemy"></param>
    virtual protected void Hit(Collider hitEnemy)
    {
        hitEnemy.GetComponent<EnemyController>().TakeDamage(damage);

    }


    /// <summary>
    /// オブジェクトを無効状態にする関数
    /// </summary>
    void Delete()
    {
        col.enabled = false;
        rig.isKinematic = true;
        transform.position = new Vector3(100, 100, 100);
        rig.velocity = new Vector3(0, 0, 0);

        isActive = false;
    }
}
