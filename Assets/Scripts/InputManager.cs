using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    // ���R���g���[���[
    [SerializeField] GameObject rightController;
    [SerializeField] GameObject leftController;

    // �R���g���[���[����o�郌�[�U�[
    [SerializeField] LineRenderer leftRayObject;
    [SerializeField] LineRenderer rightRayObject;

    // �v���C���[
    [SerializeField] GameObject VRPlayer;

    // ��x�̉E�X�e�B�b�N���͂ŉ�]����p�x
    [SerializeField] int rotateRatio = 45;

    // CompareTags�֐��Ŏg�p����A�^�C���̑S�^�O���X�g
    string[] tileTags = { "Tile", "Tile_None", "Tile_Road", "Tile_CanBuild", "Tile_EnemyBase", "Tile_PlayerBase" };

    // �������A�^���[�̑S�^�O���X�g
    string[] towerTags = { "Tower" };


    // �R�[���o�b�N�̊֌W�ňړ����Ɏg���ꎞ�ϐ�
    Vector3 tempMovePos;


    void Update()
    {
        // �e�|�C���^�[�I�u�W�F�N�g�̈ړ�
        leftRayObject.positionCount = 2;
        leftRayObject.SetPosition(0, leftController.transform.position);
        leftRayObject.SetPosition(1, leftController.transform.position + leftController.transform.forward * 20f);

        rightRayObject.positionCount = 2;
        rightRayObject.SetPosition(0, rightController.transform.position);
        rightRayObject.SetPosition(1, rightController.transform.position + rightController.transform.forward * 20f);

        // �E�l�����w�{�^��
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(rightController.transform.position, rightController.transform.forward, 100.0f);

            foreach (RaycastHit hit in hits)
            {
                if (!(CompareTags(tileTags, hit.collider.tag) || CompareTags(towerTags, hit.collider.tag)))
                    continue;

                if (hit.collider.CompareTag("Tile_None") || hit.collider.CompareTag("Tile_CanBuild"))
                {
                    hit.collider.GetComponent<TileController>().StartBuild();
                }
                else if (hit.collider.CompareTag("Tower"))
                {
                    hit.collider.GetComponent<TowerFloorController>().StartBuild();
                }

                break;
            }
        }

        // ���l�����w�{�^��
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(leftController.transform.position, leftController.transform.forward, 100.0f);
            foreach (RaycastHit hit in hits)
            {
                if (CompareTags(tileTags, hit.collider.tag))
                {
                    tempMovePos = hit.collider.GetComponent<TileController>().GetPos();
                    FadeManager.Instance.onFadeInComplete.AddListener(PlayerMove);
                    FadeManager.Instance.Fade();

                    break;
                }
            }
        }

        // �E�X�e�B�b�N���E
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            Vector2 temp = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            if (temp.x > 0)
                temp.x = 1f;
            else
                temp.x = -1f;

            VRPlayer.transform.Rotate(new Vector3(0f, temp.x * rotateRatio, 0f));
        }



        DebugKeyInput();
        
    }


    /// <summary>
    /// �ړ������ŃR�[���o�b�N���g�����߂ɕ��������֐��B
    /// </summary>
    void PlayerMove()
    {
        VRPlayer.transform.position = tempMovePos;
    }


    /// <summary>
    /// �����^�O�̂ǂꂩ�Ɉ�v���邩���ׂ�֐��B
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    bool CompareTags(string[] tags, string targetTag)
    {
        foreach (string t in tags)
        {
            if ((targetTag) == t)
                return true;
        }

        return false;

    }


    /// <summary>
    /// PC�f�o�b�O�p�̃L�[���͂��܂Ƃ߂��֐��B
    /// </summary>
    void DebugKeyInput()
    {
        // �v���C���[�̈ړ�����
        float moveRatio = 0.025f;
        float rotRatio = 1f;

        if (Input.GetKey(KeyCode.RightArrow))
            VRPlayer.transform.Rotate(new Vector3(0, rotRatio, 0), Space.World);
        if (Input.GetKey(KeyCode.LeftArrow))
            VRPlayer.transform.Rotate(new Vector3(0, -rotRatio, 0), Space.World);
        if (Input.GetKey(KeyCode.UpArrow))
            VRPlayer.transform.Rotate(new Vector3(-rotRatio, 0, 0));
        if (Input.GetKey(KeyCode.DownArrow))
            VRPlayer.transform.Rotate(new Vector3(rotRatio, 0, 0));
        if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.RawButton.LThumbstickUp))
            VRPlayer.transform.Translate(0, 0, moveRatio);
        if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.RawButton.LThumbstickDown))
            VRPlayer.transform.Translate(0, 0, -moveRatio);
        if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.RawButton.LThumbstickLeft))
            VRPlayer.transform.Translate(-moveRatio, 0, 0);
        if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.RawButton.LThumbstickRight))
            VRPlayer.transform.Translate(moveRatio, 0, 0);
        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.RawButton.Y))
            VRPlayer.transform.Translate(0, moveRatio, 0, Space.World);
        if (Input.GetKey(KeyCode.LeftShift) || OVRInput.Get(OVRInput.RawButton.X))
            VRPlayer.transform.Translate(0, -moveRatio, 0, Space.World);


        // �Q�[���̃f�o�b�O�R�}���h
        if (Input.GetKeyDown(KeyCode.F))
        {
            FadeManager.Instance.Fade();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyManager.Instance.CreateEnemy();
        }


        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            EnemyManager.Instance.CreateEnemy();
        }

        
    }

}
