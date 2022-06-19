using UnityEngine;

public class UICanvasController : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    void Update()
    {
        transform.LookAt(targetCamera.transform);
    }
}
