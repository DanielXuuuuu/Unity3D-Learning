using UnityEngine;
using System.Collections;

public class GameEventManager : MonoBehaviour
{
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;

    public delegate void GameOverEvent();
    public static event GameOverEvent GameOver;

    public delegate void CoinEvent();
    public static event CoinEvent CoinNumberChange;

    //玩家逃脱
    public void PlayerEscape()
    {
        if (ScoreChange != null)
        {
            ScoreChange();
        }
    }
    //玩家被捕
    public void PlayerArrested()
    {
        if (GameOver != null)
        {
            GameOver();
        }
    }

    public void RecudeCoinNum()
    {
        if(CoinNumberChange != null)
        {
            CoinNumberChange();
        }
    }

    public void TimeOut()
    {
        if (GameOver != null)
        {
            GameOver();
        }
    }
}
