using UnityEngine;

namespace Bullet
{
    public class ShockWaveController : BulletController
    {
        Animator myAnimator_toChangeSize;

        protected override void Awake()
        {
            base.Awake();
            myAnimator_toChangeSize = GetComponent<Animator>();
            isThroughEnemy_toBranchDeleteFunc = true;
        }

        protected override void FixedUpdate() { }


        public override void Init(TowerFloorController tfc_toGetEnemyAndBarrelData, int damage_toDealEnemy = 5)
        {
            transform.position = tfc_toGetEnemyAndBarrelData.transform.position;

            myAnimator_toChangeSize.SetTrigger("AnimTrigger");

            isActive_toActivateUpdate = true;
        }


        public void OnAnimationEnd()
        {
            Disable();
        }

    }
}