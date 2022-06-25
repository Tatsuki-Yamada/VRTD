using UnityEngine;

public class UICanvasController : MonoBehaviour
{
    // UICanvasをプレイヤーに向かせるためのスクリプトだが、現在調整中


    [SerializeField] Camera targetCamera;


    void Update()
    {
        transform.LookAt(targetCamera.transform);
    }
}
