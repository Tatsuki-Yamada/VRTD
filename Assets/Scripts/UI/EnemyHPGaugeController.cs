using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Enemy;

public class EnemyHPGaugeController : MonoBehaviour
{
    EnemyControllerBase parentEnemyController;

    [SerializeField] Image hpImage;


    private void Awake()
    {
        parentEnemyController = this.transform.parent.GetComponent<EnemyControllerBase>();

        // HPゲージの更新を登録する。
        parentEnemyController.CurrentHP.Subscribe(currentHP => hpImage.fillAmount = (float)currentHP / parentEnemyController.maxHP);
    }

}
