using UnityEngine;
using System.Collections;

public class RiverModel
{
    GameObject river;

    public RiverModel()
    {
        river = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/River"),
                                new Vector3(0, -1, 0), Quaternion.Euler(2, 0, 0));
        river.name = "river";
    }
}