using UnityEngine;

namespace Bullet
{
    public class PiercingBulletController : BulletController
    {
        float aliveTime_toDelete = 5f;


        protected override void Awake()
        {
            base.Awake();
            isThroughEnemy_toBranchDeleteFunc = true;
        }


        public override void Init(TowerFloorController tfc_toGetEnemyAndBarrelData, int damage_toDealEnemy = 5)
        {
            base.Init(tfc_toGetEnemyAndBarrelData);

            moveSpeed_toMultiplyMoveVec = 3f;
            transform.LookAt(tfc_toGetEnemyAndBarrelData.GetFirstTargetableEnemy().transform);

            Invoke("Disable", aliveTime_toDelete);

        }

        protected override void Move()
        {
            myRigidbody_toMove.AddRelativeForce(Vector3.forward * 3 * moveSpeed_toMultiplyMoveVec);
        }

        protected override void CheckTargetActive()
        {
        }
    }
}