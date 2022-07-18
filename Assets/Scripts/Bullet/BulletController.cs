using UnityEngine;

public class BulletController : MonoBehaviour
{
    // ターゲットのTransform
    Transform target;

    // 自分のRigidbody
    Rigidbody rig;

    // 自分のCollider
    Collider col;

    // 移動スピード
    [SerializeField] float moveSpeed = 3f;

    // 有効かを示すフラグ
    public bool isActive = true;


    int damage = 5;


    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }


    void FixedUpdate()
    {
        if (isActive)
        {
            transform.LookAt(target.position);
            rig.AddRelativeForce(Vector3.forward * moveSpeed);


            if (target.position == new Vector3(50, 50, 50))
            {
                Delete();
            }
        }
    }


    /// <summary>
    /// 飛んでいく先のオブジェクトを設定する関数
    /// </summary>
    /// <param name="obj"></param>
    public void SetTarget(GameObject obj)
    {
        target = obj.transform;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Delete();
        }
    }


    /// <summary>
    /// 無効状態のオブジェクトを有効にして再利用する関数
    /// </summary>
    /// <param name="obj"></param>
    public void Reset(GameObject obj)
    {
        SetTarget(obj);
        col.enabled = true;
        rig.isKinematic = false;

        isActive = true;
    }


    /// <summary>
    /// オブジェクトを無効状態にする関数
    /// </summary>
    void Delete()
    {
        col.enabled = false;
        rig.isKinematic = true;
        transform.position = new Vector3(100, 100, 100);
        rig.velocity = new Vector3(0, 0, 0);

        isActive = false;
    }
}
