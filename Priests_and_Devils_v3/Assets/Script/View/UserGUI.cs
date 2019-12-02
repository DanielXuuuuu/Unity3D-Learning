using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    public int sign = 0; // 0：游戏进行中， 1：游戏胜利  -1：游戏失败

    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        GUIStyle text_style;
        GUIStyle button_style;
        text_style = new GUIStyle()
        {
            fontSize = 30
        };
        button_style = new GUIStyle("button")
        {
            fontSize = 15
        };
        if (sign == 0) {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 80, 60, 60), "Go!"))
            {
                action.MoveBoat();
            }
            if (GUI.Button(new Rect(Screen.width / 2 + 40, 80, 60, 60), "Help!"))
            {
                action.AI();
            }
        }
        else if(sign == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, 100, 120, 50), "You Failed!", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, 150, 100, 50), "Try Agian", button_style))
            {
                action.Restart();
                sign = 0;
            }
        }
        else if(sign == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, 100, 120, 50), "You Win!", text_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 50, 150, 100, 50), "Restart", button_style))
            {
                action.Restart();
                sign = 0;
            }
        }
    }
}
