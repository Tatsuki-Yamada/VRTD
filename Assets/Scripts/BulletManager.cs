using System.Collections.Generic;
using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    List<BulletController> bullets;

    public void CreateBullet(Vector3 burrel)
    {
        foreach (BulletController b in bullets)
        {
            if (b.isActive)

        }
    }
}
