using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CCActionManager : SSActionManager, ISSActionCallback
{
    public CCFlyAction fly;

    protected new void Start(){ }

    //飞碟飞行
    public void playDisk(GameObject disk, float angle, float power)
    {
        fly = CCFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }

    #region ISSActionCallback implementation
    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Compeleted,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null)
    {
        //回调函数,动作执行完后调用
    }
    #endregion
}