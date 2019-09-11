using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tic_Tac_Toe : MonoBehaviour
{
    private int[,] matrix = new int[3, 3];
    private int turn = 1; //用于记录轮到哪一方了
    private bool start = false; //用于记录游戏是否开始

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    void Reset()
    {
        start = false;
        turn = 1;
        for(int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                matrix[i, j] = 0;
    }

    int Check()
    {
        // -1：平局   0：继续   1：O胜利   2：X胜利

        int temp;
        //row
        for(int i = 0; i < 3; i++)
        {
            if((temp = matrix[i, 0]) != 0)
            {
                for(int j = 1; j < 3; j++)
                {
                    if (matrix[i, j] != temp)
                        break;
                    if (j == 2)
                        return temp;
                }
            }
        }

        //column
        for (int j = 0; j < 3; j++)
        {
            if ((temp = matrix[0, j]) != 0)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (matrix[i, j] != temp)
                        break;
                    if (i == 2)
                        return temp;
                }
            }
        }

        //cross
        if (matrix[0, 0] != 0 && matrix[0, 0] == matrix[1, 1] && matrix[1, 1] == matrix[2, 2])
            return matrix[0, 0];
        if (matrix[0, 2] != 0 && matrix[0, 2] == matrix[1, 1] && matrix[1, 1] == matrix[2, 0])
            return matrix[0, 2];

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (matrix[i, j] == 0)
                    return 0;
        return -1;
    }

    void OnGUI()
    {
        if (!start)
        {
            if (GUI.Button(new Rect(300, 100, 150, 50), "Start"))
            {
                start = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(300, 100, 150, 50), "Reset"))
            {
                Reset();
            }
        }

        int state = Check();
        if(state == -1)
            GUI.Label(new Rect(350, 155, 50, 50), "平局!");
        else if(state == 1)
            GUI.Label(new Rect(350, 155, 50, 50), "O 胜出!");
        else if(state == 2)
            GUI.Label(new Rect(350, 155, 50, 50), "X 胜出!");

        //绘制棋盘
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if (matrix[i, j] == 1)
                    GUI.Button(new Rect(300 + 50 * i, 180 + 50 * j, 50, 50), "O");
                else if (matrix[i, j] == 2)
                    GUI.Button(new Rect(300 + 50 * i, 180 + 50 * j, 50, 50), "X");
                else
                {
                    if (GUI.Button(new Rect(300 + 50 * i, 180 + 50 * j, 50, 50), ""))
                    {
                        if (start) //只有当点击“start”后，才能开始游戏
                        {
                            if (turn == 1)
                                matrix[i, j] = 1;
                            else
                                matrix[i, j] = 2;
                            turn = -turn;
                        }
                    }
                }
              
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
