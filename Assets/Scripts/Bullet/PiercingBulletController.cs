﻿using UnityEngine;

namespace Bullet
{
    public class PiercingBulletController : BulletController
    {
        float aliveTime = 5f;

        protected override void Awake()
        {
            base.Awake();
            isThroughEnemy_toBranchDeleteFunc = true;
        }


        public override void Init(TowerFloorController tfc_toGetEnemyAndBarrelData)
        {
            base.Init(tfc_toGetEnemyAndBarrelData);

            transform.LookAt(tfc_toGetEnemyAndBarrelData.GetFirstTargetableEnemy().transform);

            Invoke("Delete", aliveTime);

        }

        protected override void Move()
        {
            myRigidbody_toMove.AddRelativeForce(Vector3.forward * moveSpeed_toMultiplyMoveVec);
        }
    }
}