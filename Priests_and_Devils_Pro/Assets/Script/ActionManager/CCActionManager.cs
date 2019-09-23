using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCActionManager : SSActionManager, ISSActionCallback
{
    public FirstController sceneController;
    public CCMoveToAction moveBoat; 
    public CCSequenceAction moveRole; //移动角色是一个组合动作
    // Use this for initialization
    protected new void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentSceneController as FirstController;
        sceneController.actionManager = this;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    public void MoveBoat(GameObject boat, Vector3 target, float speed)
    {
        moveBoat = CCMoveToAction.GetSSAction(target, speed);
        this.RunAction(boat, moveBoat, this);
        sceneController.moving = true;
    }

    public void MoveRole(GameObject role, Vector3 middlePosition, Vector3 endPosition, float speed)
    {
        SSAction step1 = CCMoveToAction.GetSSAction(middlePosition, speed);
        SSAction step2 = CCMoveToAction.GetSSAction(endPosition, speed);
        moveRole = CCSequenceAction.GetSSAction(1, 0, new List<SSAction> { step1, step2 });
        this.RunAction(role, moveRole, this);
        sceneController.moving = true;
    }

    #region ISSActionCallback implementation
    public void SSActionEvent(SSAction source, 
        SSActionEventType events = SSActionEventType.Compeleted,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null)
    {
        //回调函数,动作执行完后调用
        sceneController.moving = false;
        
    }
    #endregion
}
