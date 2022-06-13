using UnityEngine;


public class GameFieldManager : SingletonMonoBehaviour<GameFieldManager>
{
    // �^�C����Prefab�������i�[����ϐ�
    [SerializeField] GameObject[] Tile_Prefabs = new GameObject[5];

    [SerializeField] GameObject TilesParent;

    /// <summary>
    /// �t�B�[���h���`����B
    /// 0�������A1�����A2���ݒu�\�A3���G���_�A4�������_�B
    /// </summary>
    int[,] field =
    {
        {9, 9, 9, 9, 9, 9, 9, 9, 9, },
        {9, 0, 0, 0, 1, 1, 1, 0, 9, },
        {9, 3, 1, 1, 1, 0, 1, 1, 9, },
        {9, 0, 0, 2, 0, 2, 0, 1, 9, },
        {9, 4, 1, 1, 1, 0, 0, 1, 9, },
        {9, 0, 0, 0, 1, 1, 1, 1, 9, },
        {9, 9, 9, 9, 9, 9, 9, 9, 9, },
    };

    // �G���ʂ�o�H��field�Ɠ����`�œ���ϐ��B
    public int[,] enemyPath;

    // �G���_�̍��W������ϐ��B
    public int enemyBasePosX = 0;
    public int enemyBasePosY = 0;

    // �����_�̍��W������ϐ��B
    public int playerBasePosX = 0;
    public int playerBasePosY = 0;

    // �^�C������ׂ�ۂ̃I�t�Z�b�g�B
    public float createFieldOffsetX = -4f;
    public float createFieldOffsetY = -1.5f;
    public float createFieldOffsetZ = -3f;


    // �G���ʂ�o�H��T������֐��B
    public void SearchPath()
    {
        // �n�_�i�G���_�j�ƏI�_�i�����_�j��T���B
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == 3)
                {
                    enemyBasePosX = x;
                    enemyBasePosY = y;
                }
                else if (field[y, x] == 4)
                {
                    playerBasePosX = x;
                    playerBasePosY = y;
                }
            }
        }

        // enemyPath������������B
        enemyPath = new int[field.GetLength(0), field.GetLength(1)];

        // �G���_�Ɏn�_��1��o�^����B
        enemyPath[enemyBasePosY, enemyBasePosX] = 1;

        int distance = 1; 

        // �G���_����e�}�X�̋����𑪂�B
        while (true)
        {
            distance++;

            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (enemyPath[y, x] == 0)
                    {
                        // ���������Ȃ�
                        if (field[y, x] == 1 || field[y, x] == 4)
                        {
                            // �㉺���E���o�H�̐�[�Ȃ�
                            if (enemyPath[y - 1, x] == distance - 1 ||
                                enemyPath[y + 1, x] == distance - 1 ||
                                enemyPath[y, x - 1] == distance - 1 ||
                                enemyPath[y, x + 1] == distance - 1)
                            {
                                enemyPath[y, x] = distance;
                            }

                        }
                        else if (field[y, x] != 3 && field[y, x] != 4)
                        {
                            // �ړ��s�}�X���o�H�ɑI�΂�Ȃ��悤�ɁA����99���Z�b�g����B
                            enemyPath[y, x] = 99;
                        }
                    }
                }
            }

            // �o�H�������_�܂ŐL�т���I������B
            if (enemyPath[playerBasePosY, playerBasePosX] != 0)
            {
                break;
            }
        }


        int tempX = playerBasePosX;
        int tempY = playerBasePosY;
        // ��������ŒZ�o�H��1�Ōq���B
        while (distance > 1)
        {
            enemyPath[tempY, tempX] = 1;

            if (enemyPath[tempY + 1, tempX] == distance - 1)
                tempY++;
            else if (enemyPath[tempY - 1, tempX] == distance - 1)
                tempY--;
            else if (enemyPath[tempY, tempX + 1] == distance - 1)
                tempX++;
            else if (enemyPath[tempY, tempX - 1] == distance - 1)
                tempX--;

            distance--;
        }
    }


    /// <summary>
    /// �^�C������ׂ�֐��B
    /// </summary>
    public void CreateField()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == 9)
                {
                    Instantiate(Tile_Prefabs[0], new Vector3(x + createFieldOffsetX, 0 + createFieldOffsetY, y + createFieldOffsetZ), Quaternion.identity, TilesParent.transform);
                }
                else
                {
                    Instantiate(Tile_Prefabs[field[y, x]], new Vector3(x + createFieldOffsetX, 0 + createFieldOffsetY, y + createFieldOffsetZ), Quaternion.identity, TilesParent.transform);
                }
            }
        }
    }


    private void Debug_ShowEnemyPath()
    {
        GameObject testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);


        for (int y = 0; y < enemyPath.GetLength(0); y++)
        {
            for (int x = 0; x < enemyPath.GetLength(1); x++)
            {
                if (enemyPath[y, x] == 1)
                {
                    Instantiate(testCube, new Vector3(2 * x, 0, 2 * y), Quaternion.identity);
                }
            }
        }

    }


    void Start()
    {
        SearchPath();
        CreateField();
    }
}
