using UnityEngine;
using DG.Tweening;
using System;

public class EnemyController : MonoBehaviour
{
    // �ړ��V�[�N�G���X
    Sequence moveSequence;

    // 1�^�C�������b�����Ĉړ����邩
    float moveSpeed = 2f;

    // ��]�ɉ��b�����邩
    float rotateSpeed = 0.5f;

    // �G��HP
    public int HP = 5;

    // �G�������Ă��邩�����ϐ�
    bool isActive = false;


    void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                TakeDamage(1);
            }
        }
    }


    // �U�����󂯂�֐�
    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP <= 0 && isActive)
        {
            moveSequence.Kill();
            isActive = false;

            transform.position = new Vector3(50, 50, 50);
        }
    }


    // �������܂��͍ė��p���ɍs������������
    public void Reset(int enemyBaseX, int enemyBaseY, int[,] enemyPath)
    {
        isActive = true;

        int x = enemyBaseX;
        int y = enemyBaseY;

        int[,] path = new int[enemyPath.GetLength(0), enemyPath.GetLength(1)];
        Array.Copy(enemyPath, path, enemyPath.Length);

        string lastDirection = "";
        bool firstRotation = true;

        moveSequence = DOTween.Sequence();

        while (true)
        {
            // �E����
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
            // ������
            else if(path[y, x - 1] == 1)
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
            // �����
            else if (path[y + 1, x] == 1)
            {
                if (lastDirection != "Up")
                {
                    if (firstRotation)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                        firstRotation= false;
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
            // ������
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
            // �㉺���E�ɍs���悪�����Ȃ�����I��
            else
            {
                break;
            }
        }
    }
}
