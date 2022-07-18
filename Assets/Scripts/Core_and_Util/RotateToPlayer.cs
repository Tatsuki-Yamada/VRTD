using UnityEngine;

/// <summary>
/// プレイヤーの方向に回転するだけのスクリプト。
/// </summary>
public class RotateToPlayer : MonoBehaviour
{
    [SerializeField] bool isReverse = false;

    Camera targetCamera;

    void Awake()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(targetCamera.transform.position);

        if (isReverse)
            transform.Rotate(0, 180, 0);
    }
}
