using UnityEngine;
using System.Collections.Generic;

public class TowerFloorController : MonoBehaviour
{
    // ���˂���e��Prefab
    [SerializeField] GameObject bulletPrefab;

    // ���˂���C�g��Transform
    [SerializeField] Transform barrelTransform;

    // �G���U���͈͓��ɂ��邩�����ϐ�
    bool enemiesInRange = false;

    // �ˌ����Ă���o�߂�������
    float timeFromLastShot = 0f;

    // �����̃����[�h�ɂ����鎞��
    float reloadTime = 1f;

    // �U���͈͓��ɂ���G�̃��X�g
    List<GameObject> targetEnemyList = new List<GameObject>();

    // �U���͈͂��Ǘ�����q�X�N���v�g
    TowerRangeController rangeController;


    void Awake()
    {
        rangeController = transform.GetChild(0).GetComponent<TowerRangeController>();
    }


    void Update()
    {
        if (enemiesInRange)
        {
            if (!(targetEnemyList[0].transform.position == new Vector3(50, 50, 50)))
            {
                transform.LookAt(targetEnemyList[0].transform.position);
                transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z));
            }
        }

        Shoot();
    }


    // �U���\���X�g�ɒǉ����鏈��
    public void SetTargetEnemy(GameObject enemy)
    {
        enemiesInRange = true;
        targetEnemyList.Add(enemy);
    }


    // �U���\���X�g���珜�O���鏈��
    public void RemoveTargetEnemy(GameObject enemy)
    {
        int i = targetEnemyList.IndexOf(enemy);
        if (i != -1)
        {
            targetEnemyList.RemoveAt(i);
        }

        if (targetEnemyList.Count == 0)
        {
            enemiesInRange = false;
        }
    }

    void Shoot()
    {
        
        timeFromLastShot += Time.deltaTime;

        if (enemiesInRange)
        {
            if (timeFromLastShot > reloadTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, transform.rotation);
                bullet.GetComponent<BulletController>().SetTarget(targetEnemyList[0]);
                timeFromLastShot = 0f;
            }
        }
        
    }
}
