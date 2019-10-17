using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    private string score, round;
    int blood, gameState, HighestScore;

    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    // Update is called once per frame
    void Update()
    {
        gameState = action.GetGameState();
    }

    void OnGUI()
    {
        GUIStyle text_style;
        GUIStyle button_style;
        text_style = new GUIStyle()
        {
            fontSize = 20
        };
        button_style = new GUIStyle("button")
        {
            fontSize = 15
        };

        if (gameState == 0)
        {
            //初始界面
            if (GUI.Button(new Rect(Screen.width / 2 - 50, 80, 100, 60), "Start Game", button_style))
            {
                action.StartGame();
            }
        }
        else if(gameState == 1)
        {
            //游戏进行中
            //用户射击
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePos = Input.mousePosition;
                action.Hit(mousePos);
            }

            score = "Score: " + action.GetScore().ToString();
            GUI.Label(new Rect(200, 5, 100, 100), score, text_style);
            round = "Round: " + action.GetCurrentRound().ToString();
            GUI.Label(new Rect(400, 5, 100, 100), round, text_style);

            blood = action.GetBlood();
            string bloodStr = "Blood: " + blood.ToString();
            GUI.Label(new Rect(600, 5, 50, 50), bloodStr, text_style);
        }
        else
        {
            //游戏结束，有两种情况
            if (gameState == 2)
            {

                if (action.GetScore() > HighestScore) {
                    HighestScore = action.GetScore();
                }       
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 250, 100, 60), "Game Over", text_style);
                string record = "Highest Score: " + HighestScore.ToString();  
                GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 150, 150, 60), record, text_style);
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 70), "You Lost!", text_style);
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 30, 100, 60), "Restart", button_style))
            {
                action.Restart();
            }
        }
    }     
}