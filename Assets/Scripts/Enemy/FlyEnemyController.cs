using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class FlyEnemyController : EnemyControllerBase
    {
        public override void Init()
        {
            base.Init();

            transform.Translate(0, 1, 0);
        }
    }
}
