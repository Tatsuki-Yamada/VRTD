using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    // �L�����������t���O
    bool isActive = false;

    // ���Z�b�g����Ă���o�߂�������
    float elapsedTime = 0f;

    // �e�A�j���[�V�����̏��v����
    float fadeInTime = 0f;
    float waitTime = 0f;
    float fadeOutTime = 0f;

    // �t�F�[�h�Ɏg�������摜
    [SerializeField] Image FadeImage;

    // �R�[���o�b�N�֐��̓��ꕨ
    [System.NonSerialized] public UnityEvent onFadeInComplete = new UnityEvent();
    [System.NonSerialized] public UnityEvent onWaitComplete = new UnityEvent();
    [System.NonSerialized] public UnityEvent onFadeOutComplete = new UnityEvent();


    /// <summary>
    /// �A�N�e�B�u���̓A�j���[�V�����������s���B
    /// </summary>
    void Update()
    {
        if (isActive)
        {
            elapsedTime += Time.deltaTime;

            // �t�F�[�h�C�����̏���
            if (elapsedTime < fadeInTime)
            {
                FadeImage.color = new Color(0, 0, 0, 1 * (elapsedTime / fadeInTime));
            }
            // �t�F�[�h�C��������̑ҋ@���Ԓ��̏���
            else if(elapsedTime < fadeInTime + waitTime)
            {
                onFadeInComplete.Invoke();
            }
            // �t�F�[�h�A�E�g���̏���
            else if (elapsedTime < fadeInTime + waitTime + fadeOutTime)
            {
                FadeImage.color = new Color(0, 0, 0, 1 - (1 * (elapsedTime / (fadeInTime + waitTime + fadeOutTime))));
                onWaitComplete.Invoke();
            }
            // �S�Ă̗��ꂪ�I�������Ƃ��̏���
            else
            {
                onFadeOutComplete.Invoke();
                isActive = false;
            }
        }
    }


    /// <summary>
    /// �t�F�[�h���n�߂�֐��B�e�A�j���[�V�����̎��Ԃ��w�肷�邱�Ƃ��B
    /// </summary>
    /// <param name="fadeInTime"></param>
    /// <param name="waitTime"></param>
    /// <param name="fadeOutTime"></param>
    public void Fade(float fadeInTime = 0.5f, float waitTime = 0.25f, float fadeOutTime = 0.5f)
    {
        // ���Ԋ֌W�̃��Z�b�g
        elapsedTime = 0f;
        this.fadeInTime = fadeInTime;
        this.waitTime = waitTime;
        this.fadeOutTime = fadeOutTime;

        isActive = true;
    }
}
