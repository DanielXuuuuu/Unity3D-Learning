using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//追踪动作
public class PatrolFollowAction : SSAction
{     
    private GameObject player;        // 创建动作时传入玩家对象，以便对玩家进行追踪
    private float speed = 3f;         // 追踪玩家的速度
    private PatrolData data;          // 巡逻兵数据

    public static PatrolFollowAction GetSSAction(GameObject player)
    {
        PatrolFollowAction action = CreateInstance<PatrolFollowAction>();
        action.player = player;

        return action;
    }

    public override void Start()
    {
        data = this.gameObject.GetComponent<PatrolData>();
    }

    public override void Update()
    {
        if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
        if (transform.position.y != 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

        transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //追踪时面朝玩家
        this.transform.LookAt(player.transform.position);

        //丢失目标，停止追踪
        //如果侦察兵没有跟随对象，或者需要跟随的玩家不在侦查兵的区域内
        if (data.manageFloor != data.plyerFloor)
        {
            this.destory = true;
            this.callback.SSActionEvent(this, SSActionEventType.Compeleted, 1, "stop follow", this.gameObject);
        }
    }
}
