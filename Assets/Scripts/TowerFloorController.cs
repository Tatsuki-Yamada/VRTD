using UnityEngine;

public class TowerFloorController : MonoBehaviour
{
    // ”­ŽË‚·‚é’e‚ÌPrefab
    [SerializeField] GameObject bulletPrefab;

    // “G‚ªUŒ‚”ÍˆÍ“à‚É‚¢‚é‚©Ž¦‚·•Ï”
    bool enemiesInRange = false;

    // ŽËŒ‚‚µ‚Ä‚©‚çŒo‰ß‚µ‚½ŽžŠÔ
    float timeFromLastShot = 0f;

    // •‘•‚ÌƒŠƒ[ƒh‚É‚©‚©‚éŽžŠÔ
    float reloadTime = 1f;

    // UŒ‚‘ÎÛ‚Ì“GƒIƒuƒWƒFƒNƒg
    GameObject targetEnemy;



    void Update()
    {
        timeFromLastShot += Time.deltaTime;

        if (enemiesInRange)
        {
            if (timeFromLastShot > reloadTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
                bullet.GetComponent<BulletController>().SetTarget(targetEnemy);
                timeFromLastShot = 0f;
            }
        }
    }
}
