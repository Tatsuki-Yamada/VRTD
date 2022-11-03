using System.Collections.Generic;
using UnityEngine;

namespace Bullet
{
    public class ExplosionRangeController : MonoBehaviour
    {
        public List<GameObject> inRangedEnemies { get; private set; } = new List<GameObject>();


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                inRangedEnemies.Add(other.gameObject);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                inRangedEnemies.Remove(other.gameObject);
            }
        }

    }
}