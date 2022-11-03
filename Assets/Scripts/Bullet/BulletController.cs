using UnityEngine;


/// <summary>
/// 敵に向かって飛んでいくタイプの弾の基底クラス
/// </summary>
namespace Bullet
{
    public class BulletController : MonoBehaviour
    {
        // ターゲットのTransform
        GameObject targetEnemy_toFly;


        // 自分のRigidbody
        protected Rigidbody myRigidbody_toMove;

        // 自分のCollider
        protected Collider myCollider_toSwitchEnable;

        // 敵に与えるダメージ
        [SerializeField] protected int damage_toDealEnemy = 5;

        // 移動スピード
        [SerializeField] protected float moveSpeed_toMultiplyMoveVec = 3f;

        // 有効かを示すフラグ
        public bool isActive_toActivateUpdate = true;

        // 敵を貫通するか
        protected bool isThroughEnemy_toBranchDeleteFunc = false;


        protected virtual void Awake()
        {
            myRigidbody_toMove = GetComponent<Rigidbody>();
            myCollider_toSwitchEnable = GetComponent<Collider>();
        }


        /// <summary>
        /// 生成・再利用時の初期化処理を行う関数
        /// </summary>
        /// <param name="targetEnemy_toSetTarget"></param>
        public virtual void Init(TowerFloorController tfc_toGetEnemyAndBarrelData)
        {
            targetEnemy_toFly = tfc_toGetEnemyAndBarrelData.GetFirstTargetableEnemy();
            transform.position = tfc_toGetEnemyAndBarrelData.GetMuzzlePosition();

            myCollider_toSwitchEnable.enabled = true;
            myRigidbody_toMove.isKinematic = false;

            isActive_toActivateUpdate = true;
        }


        protected virtual void FixedUpdate()
        {
            if (isActive_toActivateUpdate)
            {
                Move();

                if (targetEnemy_toFly.transform.position == new Vector3(50, 50, 50))
                {
                    Delete();
                }
            }
        }



        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Hit(other);

                if (!isThroughEnemy_toBranchDeleteFunc)
                {
                    Delete();
                }
            }
        }


        /// <summary>
        /// 目標に向かって移動する関数
        /// </summary>
        protected virtual void Move()
        {
            transform.LookAt(targetEnemy_toFly.transform.position);
            myRigidbody_toMove.AddRelativeForce(Vector3.forward * moveSpeed_toMultiplyMoveVec);
        }


        /// <summary>
        /// 当たった敵にダメージを与える関数
        /// </summary>
        /// <param name="hitEnemy"></param>
        protected virtual void Hit(Collider hitEnemy)
        {
            hitEnemy.GetComponent<EnemyController>().TakeDamage(damage_toDealEnemy);

        }


        /// <summary>
        /// オブジェクトを無効状態にする関数
        /// </summary>
        protected void Delete()
        {
            myCollider_toSwitchEnable.enabled = false;
            myRigidbody_toMove.isKinematic = true;
            transform.position = new Vector3(100, 100, 100);
            myRigidbody_toMove.velocity = new Vector3(0, 0, 0);

            isActive_toActivateUpdate = false;
        }
    }
}