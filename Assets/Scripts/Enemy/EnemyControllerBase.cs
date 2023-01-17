using UnityEngine;
using DG.Tweening;
using UniRx;
using Interfaces;

namespace Enemy
{
    public class EnemyControllerBase : MonoBehaviour, IDamagable
    {
        // 必要なマネージャーの参照
        private GameManager gameManagerRef;
        private GameFieldManager gameFieldManagerRef;
        private EnemyManager enemyManagerRef;

        // 移動シークエンス
        private Sequence myMoveSequence;

        // 移動・回転に要する秒数
        protected float timePerMoveTile_sec = 1.5f;
        protected float timePerRotate_sec = 0.5f;

        // このオブジェクトが生きているか示すフラグ
        public bool isActive = false;

        // 最大HPと現在HP
        [System.NonSerialized]
        public int maxHP = 50;
        private IntReactiveProperty currentHP = new IntReactiveProperty(0);

        // 現在HPのObservable
        public IReadOnlyReactiveProperty<int> CurrentHP => currentHP;


        private void Awake()
        {
            SetManagerRefs();

            // Disposeを登録する。
            currentHP.AddTo(this);

            // HPが0になったときの処理を登録する。
            this.CurrentHP.Where(currentHP => currentHP <= 0).Subscribe(nouse => Disable());
        }


        /// <summary>
        /// 必要なマネージャーへの参照を登録する。
        /// </summary>
        private void SetManagerRefs()
        {
            gameManagerRef = GameManager.Instance;
            gameFieldManagerRef = GameFieldManager.Instance;
            enemyManagerRef = EnemyManager.Instance;
        }


        // 生成・再利用時に必要な初期化処理を行う。
        public virtual void Init()
        {
            isActive = true;

            this.transform.position = gameFieldManagerRef.GetEnemySpawnPos();
            currentHP.Value = maxHP;

            SetMovePath();
        }


        /// <summary>
        /// HPが0になったときに呼ばれる関数
        /// </summary>
        private void Disable()
        {
            isActive = false;
            myMoveSequence.Kill();

            transform.position = new Vector3(50, 50, 50);
        }


        public void TakeDamage(int damageAmount)
        {
            currentHP.Value -= damageAmount;
            SoundManager.Instance.PlaySound(this.transform.position, 2);
        }


        public void TakeSlow(float slowRatio)
        {
            myMoveSequence.timeScale = 1 - slowRatio / 100;
        }


        /// <summary>
        /// 敵が自拠点に到達したときの処理
        /// </summary>
        private void ReachPlayerBase()
        {
            GameManager.Instance.PlayerBaseTakeDamage();
            Disable();
        }



        /// <summary>
        /// 移動経路をセットする。
        /// </summary>
        private void SetMovePath()
        {
            int searchingPosX = gameFieldManagerRef.enemyBasePosX;
            int searchingPosZ = gameFieldManagerRef.enemyBasePosZ;

            int[,] copiedEnemyPath = new int[gameFieldManagerRef.enemyPath.GetLength(0), gameFieldManagerRef.enemyPath.GetLength(1)];
            System.Array.Copy(gameFieldManagerRef.enemyPath, copiedEnemyPath, gameFieldManagerRef.enemyPath.Length);

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
    }
}