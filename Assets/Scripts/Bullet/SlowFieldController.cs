using UnityEngine;

public class SlowFieldController : EnergyFieldController
{
    // 範囲内に入った敵をスローにする
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeSlow(0.5f);
        }
    }


    // 範囲外に出た敵のスローを解除する
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().CureSlow();
        }
    }
}
