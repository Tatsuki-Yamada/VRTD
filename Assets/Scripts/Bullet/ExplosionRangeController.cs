using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ExplosionBulletのダメージを与える敵を管理するだけのコントローラ
/// </summary>
namespace Bullet
{
    public class ExplosionRangeController : MonoBehaviour
    {
        public List<GameObject> inRangedEnemies_toSpreadDamage { get; private set; } = new List<GameObject>();


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                inRangedEnemies_toSpreadDamage.Add(other.gameObject);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                inRangedEnemies_toSpreadDamage.Remove(other.gameObject);
            }
        }

    }
}