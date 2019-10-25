using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolActionManager : SSActionManager, ISSActionCallback
{
    //巡逻动作
    private PatrolMoveAction move;
    private SceneController sceneController;

    protected new void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentSceneController as SceneController;
    }

    public void PatrolMove(GameObject patrol)
    {
        move = PatrolMoveAction.GetSSAction(patrol.transform.position);
        this.RunAction(patrol, move, this);
    }

    #region ISSActionCallback implementation
    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Compeleted,
        int intParam = 0,
        string strParam = null,
        GameObject objectParam = null)
    {
        //回调函数,动作执行完后调用
        if (intParam == 0)
        {
            //开始跟随玩家
            PatrolFollowAction follow = PatrolFollowAction.GetSSAction(sceneController.player);
            this.RunAction(objectParam, follow, this);
        }
        else
        {
            //丢失目标，继续巡逻
            PatrolMoveAction move = PatrolMoveAction.GetSSAction(objectParam.gameObject.GetComponent<PatrolData>().initPosition);
            this.RunAction(objectParam, move, this);
            //玩家逃脱
            Singleton<GameEventManager>.Instance.PlayerEscape();
        }
    }
    #endregion
}
