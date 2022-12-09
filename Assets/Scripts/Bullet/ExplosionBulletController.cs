using UnityEngine;

namespace Bullet
{
    public class ExplosionBulletController : BulletController
    {
        // 爆発範囲を示すオブジェクトのコントローラー
        [SerializeField] ExplosionRangeController rangeController_toSpreadDamage;

        [SerializeField] float explosionRange_Radius = 10f;

        override protected void Init(TowerFloorController tfc_toGetEnemyAndMuzzleData, int damage_toDealEnemy = 5, float explosionRange_toMultiply = 1f)
        {
            base.Init(tfc_toGetEnemyAndMuzzleData, damage_toDealEnemy);
            rangeController_toSpreadDamage.gameObject.transform.localScale = new Vector3(explosionRange_Radius * explosionRange_toMultiply, explosionRange_Radius, explosionRange_toMultiply, explosionRange_Radius, explosionRange_toMultiply);
        }
    }

    // ダメージを拡散させる処理のため、 Overrideしている。
    override protected void Hit(Collider hitEnemy)
    {
        foreach (GameObject enemy_toTakeDamage in rangeController_toSpreadDamage.inRangedEnemies_toSpreadDamage)
        {
            enemy_toTakeDamage.GetComponent<EnemyController>().TakeDamage(damage_toDealEnemy);
        }
    }
}
