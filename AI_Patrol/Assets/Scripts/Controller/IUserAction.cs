using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void Restart();
    void MovePlayer(float x, float z);
    int GetScore();
    int GetCoinNumHave();
    int GetCoinNumNeed();
    string GetTime();
    bool isGameOver();

}