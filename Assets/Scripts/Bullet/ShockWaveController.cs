using UnityEngine;

public class ShockWaveController : MonoBehaviour
{
    Animator animator;
    public bool isActive = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void Init(Vector3 pos)
    {
        transform.position = pos;

        animator.SetTrigger("AnimTrigger");

        isActive = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(1);
        }
    }


    public void OnAnimationEnd()
    {
        transform.position = new Vector3(250, 250, 250);

        isActive = false;
    }


}
