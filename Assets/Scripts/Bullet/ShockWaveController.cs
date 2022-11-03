using UnityEngine;

namespace Bullet
{
    public class ShockWaveController : BulletController
    {
        Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            isThroughEnemy_toBranchDeleteFunc = true;
        }

        protected override void FixedUpdate() { }


        public override void Init(TowerFloorController tfc_toGetEnemyAndBarrelData)
        {
            transform.position = tfc_toGetEnemyAndBarrelData.transform.position;

            animator.SetTrigger("AnimTrigger");

            isActive_toActivateUpdate = true;
        }


        public void OnAnimationEnd()
        {
            Delete();
        }

    }
}