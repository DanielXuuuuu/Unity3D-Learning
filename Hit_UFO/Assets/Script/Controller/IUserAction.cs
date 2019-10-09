using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void StartGame();
    void Restart();
    void Hit(Vector3 position);
    int GetScore();
    int GetCurrentRound();
    int GetBlood();
    int GetGameState();
}