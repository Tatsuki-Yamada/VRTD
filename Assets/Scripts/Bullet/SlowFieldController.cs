using UnityEngine;

namespace Bullet
{
    public class SlowFieldController : BulletController
    {
        public override void Init(TowerFloorController tfc_toGetEnemyAndBarrelData)
        {
            transform.position = tfc_toGetEnemyAndBarrelData.transform.position;

            isActive_toActivateUpdate = true;
        }

        protected override void FixedUpdate() { }


        // 範囲内に入った敵をスローにする
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().TakeSlow(0.5f);
            }
        }


        // 範囲外に出た敵のスローを解除する
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyController>().CureSlow();
            }
        }
    }
}