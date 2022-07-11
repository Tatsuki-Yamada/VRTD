using UnityEngine;

public class ConstructionSiteController : MonoBehaviour
{
    BuildCounterController targetBcc;
    int count = 0;


    /// <summary>
    /// ConstructionSiteが生成される際、この関数を通してカウンターが受け渡される
    /// </summary>
    /// <param name="bcc"></param>
    /// <param name="needCount"></param>
    public void SetBcc(BuildCounterController bcc, int needCount)
    {
        targetBcc = bcc;
        count = needCount;
    }


    /// <summary>
    /// ハンマーで叩いたときの処理をまとめた関数
    /// </summary>
    public void Hit()
    {
        targetBcc.DecreaseCount();
        count--;
        if (count <= 0)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hammer"))
        {
            Hit();
        }
    }
}
