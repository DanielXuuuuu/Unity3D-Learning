using UnityEngine;
using System.Collections;

public class LandModel
{
    GameObject land;
    public LandModel(string position)
    {
        if (position == "left")
        {
            land = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Land"),
                                new Vector3(-6.5F, -1, 0), Quaternion.identity);
            land.name = "leftLand";
        }
        else
        {
            land = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Land"),
                                new Vector3(6.5F, -1, 0), Quaternion.identity);
            land.name = "rightLand";
        }
    }
}