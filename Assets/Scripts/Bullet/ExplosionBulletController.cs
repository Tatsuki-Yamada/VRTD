using UnityEngine;

namespace Bullet
{
    public class ExplosionBulletController : BulletController
    {
        // 爆発範囲を示すオブジェクトのコントローラー
        [SerializeField] ExplosionRangeController rangeController;


        override protected void Hit(Collider hitEnemy)
        {
            foreach (GameObject enemy in rangeController.inRangedEnemies)
            {
                enemy.GetComponent<EnemyController>().TakeDamage(damage_toDealEnemy);
            }
        }
    }
}