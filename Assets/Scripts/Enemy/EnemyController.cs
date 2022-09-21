using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EnemyController : MonoBehaviour
{
    // HPバー
    [SerializeField] Image HPGauge;

    // 移動シークエンス
    Sequence moveSequence;

    // 1タイルを何秒かけて移動するか
    float moveSpeed = 1.5f;

    // 回転に何秒かけるか
    float rotateSpeed = 0.5f;

    // 最大HP
    public int maxHP = 100;

    // 現在のHP
    public int currentHP = 100;

    // 敵が生きているか示す変数
    public bool isActive = false;


    /// <summary>
    /// 生成時または再利用時に行う初期化処理
    /// </summary>
    /// <param name="enemyBaseX"></param>
    /// <param name="enemyBaseY"></param>
    /// <param name="enemyPath"></param>
    public void Init(int enemyBaseX, int enemyBaseY, int[,] enemyPath)
    {
        SetPath(enemyBaseX, enemyBaseY, enemyPath);

        currentHP = maxHP;
        UpdateHPGauge();

        isActive = true;
    }


    /// <summary>
    /// 攻撃を受ける関数
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        UpdateHPGauge();

        if (currentHP <= 0 && isActive)
        {
            Kill();
        }
    }


    /// <summary>
    /// 移動速度を割合で減少させる関数
    /// </summary>
    /// <param name="rate"></param>
    public void TakeSlow(float rate)
    {
        moveSequence.timeScale = rate;
    }


    /// <summary>
    /// 移動速度を元に戻す関数
    /// </summary>
    public void CureSlow()
    {
        TakeSlow(1f);
    }


    /// <summary>
    /// HPバーを更新する関数
    /// </summary>
    void UpdateHPGauge()
    {
        HPGauge.fillAmount = (float)currentHP / maxHP;
    }


    /// <summary>
    /// この敵が死ぬ関数
    /// </summary>
    void Kill()
    {
        moveSequence.Kill();
        isActive = false;

        transform.position = new Vector3(50, 50, 50);
    }


    /// <summary>
    /// 敵が自拠点に到達したときの処理
    /// </summary>
    void ReachPlayerBase()
    {
        GameManager.Instance.PlayerBaseTakeDamage();

        Kill();
    }


    /// <summary>
    /// 敵が辿る経路をセットする関数
    /// </summary>
    /// <param name="enemyBaseX"></param>
    /// <param name="enemyBaseY"></param>
    /// <param name="enemyPath"></param>
    void SetPath(int enemyBaseX, int enemyBaseY, int[,] enemyPath)
    {
        int x = enemyBaseX;
        int y = enemyBaseY;

        int[,] path = new int[enemyPath.GetLength(0), enemyPath.GetLength(1)];
        Array.Copy(enemyPath, path, enemyPath.Length);

        string lastDirection = "";
        bool firstRotation = true;

        moveSequence = DOTween.Sequence();
        moveSequence.OnComplete(ReachPlayerBase);

        while (true)
        {
            // 右方向
            if (path[y, x + 1] == 1)
            {
                if (lastDirection != "Right")
                {
                    if (firstRotation)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 270f, 0));
                        firstRotation = false;
                    }
                    else
                    {
                        moveSequence.Append(
                            transform.DORotate(new Vector3(0f, 270f, 0f), rotateSpeed)
                        );
                    }
                }

                moveSequence.Append(
                    transform.DOMoveX(1f, moveSpeed).SetRelative().SetEase(Ease.Linear)
                );

                path[y, x] = 0;
                x++;
                lastDirection = "Right";
            }
            // 左方向
            else if (path[y, x - 1] == 1)
            {
                if (lastDirection != "Left")
                {
                    if (firstRotation)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
                        firstRotation = false;
                    }
                    else
                    {
                        moveSequence.Append(
                            transform.DORotate(new Vector3(0f, 90f, 0f), rotateSpeed)
                        );
                    }
                }

                moveSequence.Append(
                    transform.DOMoveX(-1f, moveSpeed).SetRelative().SetEase(Ease.Linear)
                );
                path[y, x] = 0;
                x--;
                lastDirection = "Left";
            }
            // 上方向
            else if (path[y + 1, x] == 1)
            {
                if (lastDirection != "Up")
                {
                    if (firstRotation)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                        firstRotation = false;
                    }
                    else
                    {
                        moveSequence.Append(
                            transform.DORotate(new Vector3(0f, 180f, 0f), rotateSpeed)
                        );
                    }
                }

                moveSequence.Append(
                    transform.DOMoveZ(1f, moveSpeed).SetRelative().SetEase(Ease.Linear)
                );
                path[y, x] = 0;
                y++;
                lastDirection = "Up";
            }
            // 下方向
            else if (path[y - 1, x] == 1)
            {
                if (lastDirection != "Down")
                {
                    if (firstRotation)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0f, 0));
                        firstRotation = false;
                    }
                    else
                    {
                        moveSequence.Append(
                            transform.DORotate(new Vector3(0f, 0f, 0f), rotateSpeed)
                        );
                    }
                }

                moveSequence.Append(
                    transform.DOMoveZ(-1f, moveSpeed).SetRelative().SetEase(Ease.Linear)
                );
                path[y, x] = 0;
                y--;
                lastDirection = "Down";
            }
            // 上下左右に行き先が無くなったら終了
            else
            {
                break;
            }
        }
    }
}
