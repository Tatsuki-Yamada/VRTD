using UnityEngine;

public class PiercingBulletController : BulletController
{
    float aliveTime = 5f;

    protected override void Awake()
    {
        base.Awake();
        isThroughEnemy = true;
    }


    public override void Init(GameObject tgt, Vector3 pos)
    {
        base.Init(tgt, pos);

        transform.LookAt(tgt.transform);

        Invoke("Delete", aliveTime);
        
    }

    protected override void Move()
    {
        rig.AddRelativeForce(Vector3.forward * moveSpeed);
    }
}
