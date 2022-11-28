using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Image myHpGauge_toChangeFillAmount;

    Sequence myMoveSequence;

    float timePerMoveTile_sec = 1.5f;
    float timePerRotate_sec = 0.5f;

    public int maxHP_toCompareCurrentHP = 100;
    public int currentHP_toCompareMaxHP = 100;

    public bool isActive_toJudgeReuse = false;


    public void Init(float offsetY_toAddDefaultPosition = 0f)
    {
        this.transform.position = GameFieldManager.Instance.GetEnemySpawnPos() + new Vector3(0, offsetY_toAddDefaultPosition, 0);

        maxHP_toCompareCurrentHP = (int)EnemyManager.Instance.enemyBaseHP_toIncreaseByWave;
        currentHP_toCompareMaxHP = maxHP_toCompareCurrentHP;
        UpdateHPGauge();

        SetMovePath();

        isActive_toJudgeReuse = true;

        Debug.Log(maxHP_toCompareCurrentHP);
    }


    private void SetMovePath()
    {
        int searchingPosX = GameFieldManager.Instance.enemyBasePosX;
        int searchingPosZ = GameFieldManager.Instance.enemyBasePosZ;

        int[,] copiedEnemyPath = new int[GameFieldManager.Instance.enemyPath.GetLength(0), GameFieldManager.Instance.enemyPath.GetLength(1)];
        System.Array.Copy(GameFieldManager.Instance.enemyPath, copiedEnemyPath, GameFieldManager.Instance.enemyPath.Length);

        string lastDirection_toJudgeSkipRotation = "";

        myMoveSequence = DOTween.Sequence();
        myMoveSequence.OnComplete(ReachPlayerBase);

        // 最初に向かう方向にこのオブジェクトを回転させる。
        // 右方向
        if (copiedEnemyPath[searchingPosZ, searchingPosX + 1] == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
            lastDirection_toJudgeSkipRotation = "Right";
        }
        // 左方向
        else if (copiedEnemyPath[searchingPosZ, searchingPosX - 1] == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
            lastDirection_toJudgeSkipRotation = "Left";
        }
        // 上方向
        else if (copiedEnemyPath[searchingPosZ + 1, searchingPosX] == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
            lastDirection_toJudgeSkipRotation = "Up";
        }
        // 下方向
        else if (copiedEnemyPath[searchingPosZ - 1, searchingPosX] == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0f, 0));
            lastDirection_toJudgeSkipRotation = "Down";
        }

        // 経路の探索を行う。
        while (true)
        {
            // 右方向
            if (copiedEnemyPath[searchingPosZ, searchingPosX + 1] == 1)
            {
                if (lastDirection_toJudgeSkipRotation != "Right")
                {
                    myMoveSequence.Append(
                        transform.DORotate(new Vector3(0f, 270f, 0f), timePerRotate_sec)
                    );
                }

                myMoveSequence.Append(
                    transform.DOMoveX(1f, timePerMoveTile_sec).SetRelative().SetEase(Ease.Linear)
                );

                copiedEnemyPath[searchingPosZ, searchingPosX] = 0;
                searchingPosX++;
                lastDirection_toJudgeSkipRotation = "Right";
            }
            // 左方向
            else if (copiedEnemyPath[searchingPosZ, searchingPosX - 1] == 1)
            {
                if (lastDirection_toJudgeSkipRotation != "Left")
                {
                    myMoveSequence.Append(
                        transform.DORotate(new Vector3(0f, 90f, 0f), timePerRotate_sec)
                    );
                }

                myMoveSequence.Append(
                    transform.DOMoveX(-1f, timePerMoveTile_sec).SetRelative().SetEase(Ease.Linear)
                );

                copiedEnemyPath[searchingPosZ, searchingPosX] = 0;
                searchingPosX--;
                lastDirection_toJudgeSkipRotation = "Left";
            }
            // 上方向
            else if (copiedEnemyPath[searchingPosZ + 1, searchingPosX] == 1)
            {
                if (lastDirection_toJudgeSkipRotation != "Up")
                {
                    myMoveSequence.Append(
                        transform.DORotate(new Vector3(0f, 180f, 0f), timePerRotate_sec)
                    );
                }

                myMoveSequence.Append(
                    transform.DOMoveZ(1f, timePerMoveTile_sec).SetRelative().SetEase(Ease.Linear)
                );

                copiedEnemyPath[searchingPosZ, searchingPosX] = 0;
                searchingPosZ++;
                lastDirection_toJudgeSkipRotation = "Up";
            }
            // 下方向
            else if (copiedEnemyPath[searchingPosZ - 1, searchingPosX] == 1)
            {
                if (lastDirection_toJudgeSkipRotation != "Down")
                {
                    myMoveSequence.Append(
                        transform.DORotate(new Vector3(0f, 0f, 0f), timePerRotate_sec)
                    );
                }

                myMoveSequence.Append(
                    transform.DOMoveZ(-1f, timePerMoveTile_sec).SetRelative().SetEase(Ease.Linear)
                );

                copiedEnemyPath[searchingPosZ, searchingPosX] = 0;
                searchingPosZ--;
                lastDirection_toJudgeSkipRotation = "Down";
            }
            // 上下左右に行き先が無くなったら終了
            else
            {
                break;
            }
        }
    }


    /// <summary>
    /// 攻撃を受ける関数
    /// </summary>
    /// <param name="damageAmount_toDecreaseHP"></param>
    public void TakeDamage(int damageAmount_toDecreaseHP)
    {
        currentHP_toCompareMaxHP -= damageAmount_toDecreaseHP;
        UpdateHPGauge();

        if (currentHP_toCompareMaxHP <= 0)
        {
            Kill();
        }
    }


    /// <summary>
    /// 移動速度を割合で減少させる関数
    /// </summary>
    /// <param name="slowRatio_percent"></param>
    public void TakeSlow(float slowRatio_percent)
    {
        myMoveSequence.timeScale = slowRatio_percent / 100;
    }


    /// <summary>
    /// 移動速度を元に戻す関数
    /// </summary>
    public void CureSlow()
    {
        TakeSlow(100f);
    }


    /// <summary>
    /// HPバーを更新する関数
    /// </summary>
    private void UpdateHPGauge()
    {
        myHpGauge_toChangeFillAmount.fillAmount = (float)currentHP_toCompareMaxHP / maxHP_toCompareCurrentHP;
    }


    /// <summary>
    /// この敵が死ぬ関数
    /// </summary>
    private void Kill()
    {
        myMoveSequence.Kill();
        isActive_toJudgeReuse = false;

        transform.position = new Vector3(50, 50, 50);
    }


    /// <summary>
    /// 敵が自拠点に到達したときの処理
    /// </summary>
    private void ReachPlayerBase()
    {
        GameManager.Instance.PlayerBaseTakeDamage();
        Kill();
    }
}
