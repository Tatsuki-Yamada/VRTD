using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // 自拠点の残りHP
    int playerBaseHP = 5;


    // 自拠点に敵が侵入した時のダメージ処理
    public void PlayerBaseTakeDamage()
    {
        playerBaseHP -= 1;

        if (playerBaseHP <= 0)
        {
            GameOver();
        }
    }


    void GameOver()
    {
        Debug.Log("Game Over");
    }
}
