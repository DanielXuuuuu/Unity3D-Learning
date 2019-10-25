using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    private GUIStyle scoreStyle = new GUIStyle();
    private GUIStyle textStyle = new GUIStyle();
    private GUIStyle timeStyle = new GUIStyle();
    private GUIStyle gameOverStyle = new GUIStyle();
    private string timeCounter;

    // Start is called before the first frame update
    void Start()
    {

        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
        textStyle.fontSize = 16;
        scoreStyle.fontSize = 16;
        timeStyle.fontSize = 20;
        gameOverStyle.fontSize = 30;
    }
    // Update is called once per frame
    void Update()
    {
        float transitionX = Input.GetAxis("Horizontal");
        float transitionZ = Input.GetAxis("Vertical");
        //移动玩家
        action.MovePlayer(transitionX, transitionZ);
        timeCounter = action.GetTime();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 200, 50), "分数:", textStyle);
        GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), scoreStyle);
        GUI.Label(new Rect(250, 5, 200, 50), timeCounter, timeStyle);
        GUI.Label(new Rect(Screen.width - 330, 5, 50, 50), "已获得金币数:", textStyle);
        GUI.Label(new Rect(Screen.width - 210, 5, 50, 50), action.GetCoinNumHave().ToString(), scoreStyle);
        GUI.Label(new Rect(Screen.width - 170, 5, 50, 50), "还需寻找金币数:", textStyle);
        GUI.Label(new Rect(Screen.width - 40, 5, 50, 50), action.GetCoinNumNeed().ToString(), scoreStyle);
        if (action.isGameOver()) {
            if (action.GetCoinNumNeed() != 0){
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), "任务失败！", gameOverStyle);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "复活"))
                {
                    action.Restart();
                    return;
                }
            }
            else if (action.GetCoinNumNeed() == 0)
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 250, 100, 100), "任务完成！", gameOverStyle);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "重新开始"))
                {
                    action.Restart();
                    return;
                }
            }
        }
    }
}