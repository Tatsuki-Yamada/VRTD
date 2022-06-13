using UnityEngine;


public class GameFieldManager : SingletonMonoBehaviour<GameFieldManager>
{
    // タイルのPrefabたちを格納する変数
    [SerializeField] GameObject[] Tile_Prefabs = new GameObject[5];

    [SerializeField] GameObject TilesParent;

    /// <summary>
    /// フィールドを定義する。
    /// 0が無し、1が道、2が設置可能、3が敵拠点、4が自拠点。
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

    // 敵が通る経路がfieldと同じ形で入る変数。
    public int[,] enemyPath;

    // 敵拠点の座標が入る変数。
    public int enemyBasePosX = 0;
    public int enemyBasePosY = 0;

    // 自拠点の座標が入る変数。
    public int playerBasePosX = 0;
    public int playerBasePosY = 0;

    // タイルを並べる際のオフセット。
    public float createFieldOffsetX = -4f;
    public float createFieldOffsetY = -1.5f;
    public float createFieldOffsetZ = -3f;


    // 敵が通る経路を探索する関数。
    public void SearchPath()
    {
        // 始点（敵拠点）と終点（自拠点）を探す。
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

        // enemyPathを初期化する。
        enemyPath = new int[field.GetLength(0), field.GetLength(1)];

        // 敵拠点に始点の1を登録する。
        enemyPath[enemyBasePosY, enemyBasePosX] = 1;

        int distance = 1; 

        // 敵拠点から各マスの距離を測る。
        while (true)
        {
            distance++;

            for (int y = 0; y < field.GetLength(0); y++)
            {
                for (int x = 0; x < field.GetLength(1); x++)
                {
                    if (enemyPath[y, x] == 0)
                    {
                        // そこが道なら
                        if (field[y, x] == 1 || field[y, x] == 4)
                        {
                            // 上下左右が経路の先端なら
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
                            // 移動不可マスが経路に選ばれないように、距離99をセットする。
                            enemyPath[y, x] = 99;
                        }
                    }
                }
            }

            // 経路が自拠点まで伸びたら終了する。
            if (enemyPath[playerBasePosY, playerBasePosX] != 0)
            {
                break;
            }
        }


        int tempX = playerBasePosX;
        int tempY = playerBasePosY;
        // 距離から最短経路を1で繋ぐ。
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
    /// タイルを並べる関数。
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
