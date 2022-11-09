using UnityEngine;

/// <summary>
/// 弾すべての基底クラス
/// </summary>
namespace Bullet
{
    public class BulletController : MonoBehaviour
    {
 

        // 攻撃目標となる敵のオブジェクト
        GameObject targetEnemy_toFly;

        // 自身ののRigidbody
        protected Rigidbody myRigidbody_toMove;

        // 自身のCollider
        protected Collider myCollider_toSwitchEnable;

        // 敵に与えるダメージ
        [SerializeField] protected int damage_toDealEnemy = 5;

        // 弾が飛翔するスピード
        [SerializeField] protected float moveSpeed_toMultiplyMoveVec = 3f;

        // 弾が生きているかを示すフラグ
        public bool isActive_toActivateUpdate = true;

        // 弾が敵を貫通するかを示すフラグ
        protected bool isThroughEnemy_toBranchDeleteFunc = false;


        // Awake
        protected virtual void Awake()
        {
            myRigidbody_toMove = GetComponent<Rigidbody>();
            myCollider_toSwitchEnable = GetComponent<Collider>();
        }


        /// <summary>
        /// 生成・再利用時の初期化処理を行う関数
        /// </summary>
        /// <param name="targetEnemy_toSetTarget"></param>
        public virtual void Init(TowerFloorController tfc_toGetEnemyAndMuzzleData)
        {
            targetEnemy_toFly = tfc_toGetEnemyAndMuzzleData.GetFirstTargetableEnemy();
            transform.position = tfc_toGetEnemyAndMuzzleData.GetMuzzlePosition();

            myCollider_toSwitchEnable.enabled = true;
            myRigidbody_toMove.isKinematic = false;

            isActive_toActivateUpdate = true;
        }


        // FixedUpdate
        // フレーム数で飛翔速度が変わらないようにFixedにしている。
        protected virtual void FixedUpdate()
        {
            if (isActive_toActivateUpdate)
            {
                Move();

                if (targetEnemy_toFly.transform.position == new Vector3(50, 50, 50))
                {
                    Disable();
                }
            }
        }


        // OnTriggerEnter
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Hit(other);

                if (isThroughEnemy_toBranchDeleteFunc == false)
                {
                    Disable();
                }
            }
        }


        /// <summary>
        /// 目標に向かって飛翔する関数
        /// AddRelativeForceはオブジェクトの向いているZ軸方向に1が出る。
        /// </summary>
        protected virtual void Move()
        {
            transform.LookAt(targetEnemy_toFly.transform.position);
            myRigidbody_toMove.AddRelativeForce(Vector3.forward * moveSpeed_toMultiplyMoveVec);
        }


        /// <summary>
        /// この弾が当たった敵にダメージを与える関数
        /// </summary>
        /// <param name="hitEnemy"></param>
        protected virtual void Hit(Collider hitEnemy)
        {
            hitEnemy.GetComponent<EnemyController>().TakeDamage(damage_toDealEnemy);

        }


        /// <summary>
        /// オブジェクトを無効状態にする関数
        /// </summary>
        protected void Disable()
        {
            myCollider_toSwitchEnable.enabled = false;
            myRigidbody_toMove.isKinematic = true;
            transform.position = new Vector3(100, 100, 100);
            myRigidbody_toMove.velocity = new Vector3(0, 0, 0);

            isActive_toActivateUpdate = false;
        }
    }
}