using System.Collections;
using UnityEngine;
using Enemy;

namespace Bullet
{
    public class ExplosionBulletController : BulletController
    {
        // 爆発範囲を示すオブジェクトのコントローラー
        [SerializeField] ExplosionRangeController rangeController_toSpreadDamage;

        [SerializeField] float explosionRange_Radius = 10f;

        [SerializeField] ParticleSystem effect;

        /*
                protected void Init(TowerFloorController tfc_toGetEnemyAndMuzzleData, int damage_toDealEnemy = 5)
                {
                    base.Init(tfc_toGetEnemyAndMuzzleData, damage_toDealEnemy);
                }
                */

        public void ChangeExplosionSize(float explosionRange_toMultiply = 1f)
        {
            rangeController_toSpreadDamage.gameObject.transform.localScale = new Vector3(explosionRange_Radius * explosionRange_toMultiply, explosionRange_Radius * explosionRange_toMultiply, explosionRange_Radius * explosionRange_toMultiply);
        }


        // ダメージを拡散させる処理のため、 Overrideしている。
        override protected void Hit(Collider hitEnemy)
        {
            foreach (GameObject enemy_toTakeDamage in rangeController_toSpreadDamage.inRangedEnemies_toSpreadDamage)
            {
                enemy_toTakeDamage.GetComponent<EnemyControllerBase>().TakeDamage(damage_toDealEnemy);
            }

            SoundManager.Instance.PlaySound(this.transform.position, 3);
            effect.Play();
        }

        protected override void Disable()
        {
            Debug.Log("Disable");
            StartCoroutine(IEnuDisable());
        }


        private IEnumerator IEnuDisable()
        {
            myCollider_toSwitchEnable.enabled = false;
            myRigidbody_toMove.isKinematic = true;
            myRigidbody_toMove.velocity = new Vector3(0, 0, 0);
            this.GetComponent<Renderer>().enabled = false;

            yield return new WaitForSeconds(1);

            transform.position = new Vector3(100, 100, 100);

            isActive_toActivateUpdate = false;

            Debug.Log("IsActive");
        }
    }
}
