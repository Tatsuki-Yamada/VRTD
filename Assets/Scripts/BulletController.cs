using UnityEngine;

public class BulletController : MonoBehaviour
{
    // �Ώۂ̓G
    Transform target;

    // ���g��Rigidbody
    Rigidbody rig;

    // �e�̑��x
    [SerializeField] float moveSpeed = 3f;

    // �L�����������t���O
    public bool isActive = true;


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


    public void Reset(GameObject obj)
    {
        SetTarget(obj);

    }
}
