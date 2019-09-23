using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judger : MonoBehaviour
{
    public FirstController sceneController;
    // Use this for initialization
    public void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentSceneController as FirstController;
    }

    // Update is called once per frame
    public void Update()
    {
        if((sceneController.currentState = Judge()) != 0)
        {
            sceneController.gaming = false;
            
        }
    }

    public int Judge()
    {
        if (sceneController.moving)
            return 0;
        //计算两边的牧师和恶魔数量
        int rightPriestNum = 0, leftPriestNum = 0, rightDevilNum = 0, leftDevilNum = 0;
        for (int i = 0; i < 3; i++)
        {
            if (sceneController.Priests[i].GetSide() == 1)
            {
                rightPriestNum++;
            }
            else
            {
                leftPriestNum++;
            }

            if (sceneController.Devils[i].GetSide() == 1)
            {
                rightDevilNum++;
            }
            else
            {
                leftDevilNum++;
            }
        }
        if (leftPriestNum + leftDevilNum == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                sceneController.Devils[i].Lose();
            }
            return 1; //win
        }
        else if ((leftPriestNum > 0 && leftDevilNum > leftPriestNum) || (rightPriestNum > 0 && rightDevilNum > rightPriestNum))
        {
            int attackSide;
            if (leftDevilNum > leftPriestNum)
                attackSide = -1;
            else
                attackSide = 1;
            for (int i = 0; i < 3; i++)
            {
                if (sceneController.Devils[i].GetSide() == attackSide)
                    sceneController.Devils[i].Attack();
            }
            return -1; //lose
        }
        else
        {
            return 0;
        }
    }
}