using UnityEngine;
using System.Collections;

//物理学
public class PhysisFlyAction : SSAction
{
    private Vector3 startVector;
    public float power;

    public static PhysisFlyAction GetSSAction(Vector3 direction, float angle, float power)
    {
        PhysisFlyAction action = CreateInstance<PhysisFlyAction>();
        if (direction.x == -1)
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 1, -angle)) * Vector3.left * power;
        }
        else
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 1, angle)) * Vector3.right * power;
        }
        action.power = power;
        return action;
    }

    public override void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = power / 15 * startVector;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    public override void Update()
    {
        if (this.transform.position.y < -10)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }
}
