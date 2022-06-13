using UnityEngine;

public class BulletController : MonoBehaviour
{
    // ëŒè€ÇÃìG
    Transform target;

    // é©êgÇÃRigidbody
    Rigidbody rigidbody;

    // íeÇÃë¨ìx
    [SerializeField] float moveSpeed = 3f;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();    
    }


    void FixedUpdate()
    {
        transform.LookAt(target.position);
        rigidbody.AddRelativeForce(Vector3.forward * moveSpeed);
    }


    public void SetTarget(GameObject obj)
    {
        target = obj.transform;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
