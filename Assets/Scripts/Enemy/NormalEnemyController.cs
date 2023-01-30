using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class NormalEnemyController : EnemyControllerBase
    {
        protected override void Awake()
        {
            timePerMoveTile_sec = 2.5f;
            base.Awake();
        }
    }
}