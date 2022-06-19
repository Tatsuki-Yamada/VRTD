using UnityEngine;

public class TowerFloorController : MonoBehaviour
{
    // ���˂���e��Prefab
    [SerializeField] GameObject bulletPrefab;

    // �G���U���͈͓��ɂ��邩�����ϐ�
    bool enemiesInRange = false;

    // �ˌ����Ă���o�߂�������
    float timeFromLastShot = 0f;

    // �����̃����[�h�ɂ����鎞��
    float reloadTime = 1f;

    // �U���Ώۂ̓G�I�u�W�F�N�g
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
