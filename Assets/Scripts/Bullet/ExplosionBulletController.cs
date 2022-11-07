using UnityEngine;

namespace Bullet
{
    public class ExplosionBulletController : BulletController
    {
        // 爆発範囲を示すオブジェクトのコントローラー
        [SerializeField] ExplosionRangeController rangeController_toSpreadDamage;


        // ダメージを拡散させる処理のため、 Overrideしている。
        override protected void Hit(Collider hitEnemy)
        {
            foreach (GameObject enemy_toTakeDamage in rangeController_toSpreadDamage.inRangedEnemies_toSpreadDamage)
            {
                enemy_toTakeDamage.GetComponent<EnemyController>().TakeDamage(damage_toDealEnemy);
            }
        }
    }
}