using UnityEngine;

/// <summary>
/// プレイヤーの方向に回転するだけのスクリプト。
/// </summary>
public class RotateToPlayer : MonoBehaviour
{
    Camera targetCamera;

    void Awake()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(targetCamera.transform.position);
    }
}
