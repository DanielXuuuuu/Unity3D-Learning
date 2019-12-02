using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void MoveBoat();
    void AI();
    void MoveRole(RoleModel role);
    void Restart();
}
