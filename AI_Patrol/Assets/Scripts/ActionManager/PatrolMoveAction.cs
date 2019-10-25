using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//巡逻动作
public class PatrolMoveAction : SSAction
{
    private float posX, posZ;
    private float rectLength;                           // 边长
    private enum Dirction { EAST, NORTH, WEST, SOUTH }; // 巡逻的四个方向
    private float speed = 2f;                           // 巡逻速度
    private bool reach = true;                          // 是否到达目的地
    private Dirction dirction = Dirction.EAST;          // 移动的方向
    private PatrolData data;                            // 巡逻兵的数据

    public static PatrolMoveAction GetSSAction(Vector3 location)
    {
        PatrolMoveAction action = CreateInstance<PatrolMoveAction>();
        action.posX = location.x;
        action.posZ = location.z;

        action.rectLength = Random.Range(5, 8);
        return action;
    }

    public override void Start()
    {
        data = this.gameObject.GetComponent<PatrolData>();
    }

    public override void Update()
    {
        //防止碰撞发生后的旋转
        if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        if (transform.position.y != 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
        // 移动
        Move();
         
        // 如果所在房间相同，摧毁当前动作并回调
        if (data.manageFloor == data.plyerFloor)
        {
            this.destory = true;
            this.callback.SSActionEvent(this, SSActionEventType.Compeleted, 0 ,"follow player", this.gameObject);
        }
    }

    public void Move()
    {
        if (reach)
        {
            // 如果已到达，就换方向
            switch (dirction)
            {
                case Dirction.EAST:
                    posX -= rectLength;
                    break;
                case Dirction.NORTH:
                    posZ += rectLength;
                    break;
                case Dirction.WEST:
                    posX += rectLength;
                    break;
                case Dirction.SOUTH:
                    posZ -= rectLength;
                    break;
            }
            reach = false;
        }

        //面朝目的地
        this.transform.LookAt(new Vector3(posX, 0.5f, posZ));

        //计算距离
        float distance = Vector3.Distance(transform.position, new Vector3(posX, 0.5f, posZ));
        if (distance > 1)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(posX, 0.5f, posZ), speed * Time.deltaTime);
        }
        else
        {
            dirction = dirction + 1;
            if (dirction > Dirction.SOUTH)
            {
                dirction = Dirction.EAST;
            }
            reach = true;
        }
    }
}
