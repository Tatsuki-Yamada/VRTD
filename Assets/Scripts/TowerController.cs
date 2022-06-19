using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // �U���͈͂��Ǘ�����q�X�N���v�g
    TowerRangeController rangeController;

    // �G���U���͈͓��ɂ��邩�����ϐ�
    bool enemiesInRange = false;

    // �U���͈͓��ɂ���G�̃��X�g
    List<GameObject> targetEnemyList = new List<GameObject>();

    // �L���ɂȂ�܂ł̃p���`��
    int leftToActivate = 20;

    // �����̃����[�h�ɂ����鎞��
    float reloadTime = 1f;

    // �ˌ����Ă���o�߂�������
    float timeFromLastShot = 0f;

    // ���˂���e��Prefab
    [SerializeField] GameObject bulletPrefab;


    void Awake()
    {
        rangeController = transform.GetChild(0).GetComponent<TowerRangeController>();
    }


    void FixedUpdate()
    {
        Attack();
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


    // �U�����鏈��
    void Attack()
    {
        timeFromLastShot += Time.deltaTime;

        if (enemiesInRange)
        {
            if (timeFromLastShot > reloadTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
                bullet.GetComponent<BulletController>().SetTarget(targetEnemyList[0]);
                timeFromLastShot = 0f;
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            leftToActivate--;
        }
    }
}
