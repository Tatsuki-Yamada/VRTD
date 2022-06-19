using UnityEngine;

public class BulletController : MonoBehaviour
{
    // 対象の敵
    Transform target;

    // 自身のRigidbody
    Rigidbody rig;

    // 弾の速度
    [SerializeField] float moveSpeed = 3f;

    // 有効かを示すフラグ
    bool isActive = true;


    void Awake()
    {
        rig = GetComponent<Rigidbody>();    
    }


    void FixedUpdate()
    {
        if (isActive)
        {
            transform.LookAt(target.position);
            rig.AddRelativeForce(Vector3.forward * moveSpeed);

            if (target.position == new Vector3(50, 50, 50))
            {
                isActive = false;
                Destroy(gameObject);

            }
        }
    }


    public void SetTarget(GameObject obj)
    {
        target = obj.transform;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
