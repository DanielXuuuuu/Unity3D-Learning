using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void MoveBoat();
    void MoveRole(RoleModel role);
    int Check();
    void Restart();
}
