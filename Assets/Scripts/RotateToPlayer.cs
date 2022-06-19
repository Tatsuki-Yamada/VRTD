using UnityEngine;

/// <summary>
/// �v���C���[�̕����ɉ�]���邾���̃X�N���v�g�B
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
