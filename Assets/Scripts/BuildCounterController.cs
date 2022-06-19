using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BuildCounterController : MonoBehaviour
{
    // �\������e�L�X�g
    TextMeshPro textMesh;

    // �c��J�E���g
    int counter;

    // �L�����������t���O
    bool isActive = false;

    // �J�E���^�[��0�ɂȂ������ɌĂ΂��R�[���o�b�N
    [System.NonSerialized] public UnityEvent onCompleteBuild = new UnityEvent();

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }


    /// <summary>
    /// �J�E���^�[�����Z�b�g����֐��B
    /// </summary>
    /// <param name="num"></param>
    public void SetCount(int num)
    {
        // �J�E���^�[�̃��Z�b�g�ƃe�L�X�g�\��
        counter = num;
        textMesh.text = counter.ToString();
        GetComponent<Renderer>().enabled = true;

        isActive = true;
    }


    /// <summary>
    /// �J�E���^�[�����炷�֐��B�w�肵�Ȃ��ꍇ��1�����炷�B
    /// </summary>
    /// <param name="num"></param>
    public void DecreaseCount(int num = 1)
    {
        if (isActive)
        {
            counter -= num;
            textMesh.text = counter.ToString();

            if (counter <= 0)
            {
                onCompleteBuild.Invoke();
                GetComponent<Renderer>().enabled = false;
                isActive = false;
            }
        }


    }
}
