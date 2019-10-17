using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//运动学
public class CCFlyAction : SSAction
{
    public float gravity = -5;                                 //向下加速度
    private Vector3 startVector;                               //初速度向量
    private Vector3 gravityVector = Vector3.zero;              //加速度的向量，初始时为0
    private float time;                                        //已过去时间
    private Vector3 currentAngle = Vector3.zero;              //当前时间的欧拉角


    public static CCFlyAction GetSSAction(Vector3 direction, float angle, float power)
    {
        CCFlyAction action = CreateInstance<CCFlyAction>();
        if (direction.x == -1)
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 1, -angle)) * Vector3.left * power;
        }
        else
        {
            action.startVector = Quaternion.Euler(new Vector3(0, 1, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    public override void Update()
    {
        time += Time.fixedDeltaTime;
        gravityVector.y = gravity * time;

        transform.position += (startVector + gravityVector) * Time.fixedDeltaTime;
        currentAngle.z = Mathf.Atan((startVector.y + gravityVector.y) / startVector.x) * Mathf.Rad2Deg;
        transform.eulerAngles = currentAngle;

        if (this.transform.position.y < -10)
        {
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }
}
